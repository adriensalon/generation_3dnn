function entrainer_ia(url_ia, url_dossier_tex_label, url_dossier_tex_target, callback, ...
    continuer, utiliser_gpu)
    callback();
    fprintf('yes %f \n', 0.5);
    fprintf("yes");
    urlTempLabel = fullfile("C:\Users\adri\Desktop\temp", "entrainement_label.jpg");
    urlTempTarget = fullfile("C:\Users\adri\Desktop\temp", "entrainement_target.jpg");
    
    options = CreerOptionsEntrainement();
    if utiliser_gpu > 0
        options.ExecutionEnvironment = "gpu";
        env = @gpuArray;
    else
        options.ExecutionEnvironment = "cpu";
        env = @(x) x;
    end
    options.MaxEpochs = 200; 
    
    combinedChannels = options.InputChannels + options.OutputChannels;
    imageAndLabel = PairedImageDatastore(url_dossier_tex_label, url_dossier_tex_target, options.MiniBatchSize, ...
        "PreSize", options.PreSize, "CropSize", options.InputSize, "RandXReflection", options.RandXReflection);
    examples = imageAndLabel.shuffle();
    nExamples = 9;                              % dernier bail a regler jcrois
    examples.MiniBatchSize = nExamples;
    data = examples.read();
    thisInput = cat(4, data.A{:});
    exampleInputs = dlarray(env(thisInput), 'SSCB');
    
    imOut = (gather(extractdata(exampleInputs)) + 1)/2;
    imwrite(imtile(imOut), urlTempLabel);
        
    
    % init / charger data
    if ~continuer
        g = generator(options.InputSize, options.InputChannels, options.OutputChannels, options.GDepth);
        d = discriminator(options.InputSize, combinedChannels, options.DDepth);
        
        gOptimiser = AdamOptimiser(options.GLearnRate, options.GBeta1, options.GBeta2);
        dOptimiser = AdamOptimiser(options.DLearnRate, options.DBeta1, options.DBeta2);
        
        iteration = 0;
        startEpoch = 1;
    else
        data = load(url_ia, 'modele');
        g = data.modele.g;
        d = data.modele.d;
        gOptimiser = data.modele.gOptimiser;
        dOptimiser = data.modele.dOptimiser;
        
        iteration = gOptimiser.Iteration;
        startEpoch = floor(iteration/imageAndLabel.NumObservations)+1;
    end
    
    % pour chaque epoch
    interrompre = false;
    epoch = startEpoch;
    while epoch < options.MaxEpochs && ~interrompre
        imageAndLabel = imageAndLabel.shuffle();        
        while imageAndLabel.hasdata            
            iteration = iteration + 1;
            
            data = imageAndLabel.read();
            thisInput = cat(4, data.A{:});
            thisTarget = cat(4, data.B{:});
            
            inputImage = dlarray(env(thisInput), 'SSCB');
            targetImage = dlarray(env(thisTarget), 'SSCB');
            
            [g, gLoss, d, dLoss, lossL1, ganLoss] = ...
                dlfeval(@stepBoth, g, d, gOptimiser, dOptimiser, inputImage, targetImage, options);
                        
            if mod(iteration, options.VerboseFrequency) == 0
                logArgs = {epoch, iteration,  ...
                    gLoss, lossL1, ganLoss, dLoss};       
                interrompre = TickEntrainement(exampleInputs, g, urlTempTarget, logArgs, callback);
            end
            epoch = epoch + 1;
        end
        
        modele = struct('g', g, 'd', d, 'gOptimiser', gOptimiser, 'dOptimiser', dOptimiser);
        modele = gather(modele);
        save(url_ia, 'modele');
        assignin('base', 'modele', modele);
    end
end

function [g, gLoss, d, dLoss, lossL1, ganLoss, images] = stepBoth(g, d, gOpt, dOpt, inputImage, targetImage, options)
    
    % Make a fake image
    fakeImage = tanh(g.forward(inputImage));
    
    %% D update
    % Apply the discriminator
    realPredictions = sigmoid(d.forward(...
        cat(3, targetImage, inputImage) ...
        ));
    fakePredictions = sigmoid(d.forward(...
        cat(3, fakeImage, inputImage)...
        ));
    
    % calculate D losses
    labels = ones(size(fakePredictions), 'single');
    % crossentropy divides by nBatch, so we need to divide further
    dLoss = options.DRelLearnRate*(crossentropy(realPredictions, labels)/numel(fakePredictions(:,:,1,1)) + ...
        crossentropy(1-fakePredictions, labels)/numel(fakePredictions(:,:,1,1)));
    
    % get d gradients
    dGrads = dlgradient(dLoss, d.Learnables, "RetainData", true);
    dLoss = extractdata(dLoss);
    
    %% G update
    % to save time I just use the existing result from d
    
    % calculate g Losses
    ganLoss = crossentropy(fakePredictions, labels)/numel(fakePredictions(:,:,1,1));
    lossL1 = mean(abs(fakeImage - targetImage), 'all');
    gLoss = options.Lambda*lossL1 + ganLoss;
    
    % get g grads
    gGrads = dlgradient(gLoss, g.Learnables);
    
    % update g
    g.Learnables = dOpt.update(g.Learnables, gGrads);
    % update d
    d.Learnables = gOpt.update(d.Learnables, dGrads);
    % things for plotting
    gLoss = extractdata(gLoss);
    lossL1 = extractdata(lossL1);
    ganLoss = extractdata(ganLoss);
    
    images = {fakeImage, inputImage, targetImage};
    
end

function interrompre = TickEntrainement(exampleInputs, generator, urlFichierTexture, logArgs, callback)
   	output = tanh(generator.forward(exampleInputs));
    %nChannels = size(output, 3);
 	imOut = (gather(extractdata(output)) + 1)/2;
            
    % only take the first channel for n != 3 ENQUETER C DEGUEU CA
%     if nChannels ~= 3
%         imOut = imOut(:,:,1,:);
%     end
    
    fprintf('epoch: %d, it: %d, G: %f (L1: %f, GAN: %f), D: %f\n', logArgs{:});
    imwrite(imtile(imOut), urlFichierTexture);
    interrompre = callback(urlFichierTexture);
end

function options = CreerOptionsEntrainement()
    parser = inputParser();
    
    parser.addParameter("ExecutionEnvironment", "auto", ...
        @(x) ismember(x, ["auto", "cpu", "gpu"]));
    parser.addParameter("InputChannels", 3, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    parser.addParameter("OutputChannels", 3, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    parser.addParameter("MiniBatchSize", 1, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    parser.addParameter("RandXReflection", true, ...
        @(x) validateattributes(x, "logical", "scalar"));
    parser.addParameter("PreSize", [286, 286], ...
        @(x) validateattributes(x, "numeric", ["positive", "integer"]));
    parser.addParameter("InputSize", [256, 256], ...
        @(x) validateattributes(x, "numeric", ["positive", "integer"]));
    parser.addParameter("ARange", 255, ...
        @(x) validateattributes(x, "numeric", "positive"));
    parser.addParameter("BRange", 255, ...
        @(x) validateattributes(x, "numeric", "positive"));
    parser.addParameter("ResumeFrom", [], ...
        @(x) validateattributes(x, ["char", "string"], "scalartext"));
    parser.addParameter("GLearnRate", 0.0002, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("GBeta1", 0.5, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("GBeta2", 0.999, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("DLearnRate", 0.0002, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("DBeta1", 0.5, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("DBeta2", 0.999, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("MaxEpochs", 200, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    parser.addParameter("DRelLearnRate", 0.5, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("Lambda", 100, ...
        @(x) validateattributes(x, "numeric", "scalar"));
    parser.addParameter("GDepth", 8, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    parser.addParameter("DDepth", 4, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    parser.addParameter("Verbose", true, ...
        @(x) validateattributes(x, "logical", "scalar"));
    parser.addParameter("VerboseFrequency", 50, ...
        @(x) validateattributes(x, "numeric", ["scalar","integer","positive"]));
    
    parser.parse();
    options = parser.Results;
end
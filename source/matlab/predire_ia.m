function predire_ia(url_ia, url_tex_predict_label, url_tex_predict_target, ...
    utiliser_gpu) % rajouter parametres
    
    data = load(url_ia, 'modele');
    
    %modele = evalin('base', 'modele');
    modele = data.modele;
    inputImage = imread(url_tex_predict_label);
    inputImage = imresize(inputImage, [256, 256], "Method", "nearest");
    
    inputClass = class(inputImage);
    
    
    networkInput = PreparerImage(inputImage, utiliser_gpu);
    out = tanh(modele.g.forward(networkInput));
    
    % Make the output match the input
    translatedImage = gather((extractdata(out) + 1)/2);
    if strcmp(inputClass, "uint8")
        translatedImage = uint8(255*translatedImage);
    else
        translatedImage = cast(translatedImage, "like", inputImage);
    end

    
    imwrite(translatedImage, url_tex_predict_target);
end

function networkInput = PreparerImage(inputImage, utiliser_gpu)
    % cast to single, scale and put on the gpu as appropriate
    networkInput = 2*single(inputImage)/255 - 1; % attention arange = 255
    if (utiliser_gpu)
        networkInput = gpuArray(networkInput);
    end
    networkInput = dlarray(networkInput, 'SSCB');
end
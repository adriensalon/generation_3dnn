function out = downBlock(id, nChannels, varargin)
% downBlock    Downsampling block

% Copyright 2020 The MathWorks, Inc.

    out = block(id, nChannels, 'down', varargin{:});
    
end

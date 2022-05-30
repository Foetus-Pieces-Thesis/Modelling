
%% Import .csv File
clc, close all, clear all, format compact
XYt = xlsread("do3d-coordData-goldenStd.csv");

%% Analysis
clc, clearvars -except XYt

z0 = [2:144];
z1 = [145:301];
z2 = [302:451];
z3 = [452:599];
z4 = [600:715];

% For t=0 these are are the row ranges for each slice
% Z stack	Start Row	End Row
%   1	     2	        144
%   2	     145	    301
%   3	     302	    451
%   4	     452	    599
%   5	     600	    715

x0 = XYt(z0,4);
y0 = XYt(z0,5);
xnorm0 = zscore(x0);
ynorm0 = zscore(y0);

subplot(1,2,1)
scatter(x0,y0)
xlabel("X"),ylabel("Y")
title("Initial Positions: z=0")
grid minor

subplot(1,2,2)
scatter(xnorm0,ynorm0)
xlabel("X"),ylabel("Y")
title("Normalised Initial Positions: z=0")
grid minor



%% Fetch Coordinates for all 5 slices

% slice 1
x0 = XYt(z0,4);
y0 = XYt(z0,5);
xnorm0 = zscore(x0);
ynorm0 = zscore(y0);
% slice 2
x1 = XYt(z1,4);
y1 = XYt(z1,5);
xnorm1 = zscore(x1);
ynorm1 = zscore(y1);
% slice 3
x2 = XYt(z2,4);
y2 = XYt(z2,5);
xnorm2 = zscore(x2);
ynorm2 = zscore(y2);
% slice 4
x3 = XYt(z3,4);
y3 = XYt(z3,5);
xnorm3 = zscore(x3);
ynorm3 = zscore(y3);
% slice 5
x4 = XYt(z4,4);
y4 = XYt(z4,5);
xnorm4 = zscore(x4);
ynorm4 = zscore(y4);

%% 3D plot

figure
hold on
N = length(x0);
slice0 = -2*ones(N,1);%(0.5*randn(N,1) - 2);
scatter3(x0,y0,slice0)

N = length(x1);
slice1 = -1*ones(N,1);%(0.5*randn(N,1) - 1);
scatter3(x1,y1,slice1)

N = length(x2);
slice2 = 0*ones(N,1);%(0.5*randn(N,1) + 0);
scatter3(x2,y2,slice2)

N = length(x3);
slice3 = 1*ones(N,1);%(0.5*randn(N,1) + 1);
scatter3(x3,y3,slice3)

N = length(x4);
slice4 = 2*ones(N,1);%(0.5*randn(N,1) + 2);
scatter3(x4,y4,slice4)

xlabel("X"),ylabel("Y"),zlabel("slice (Z)");
title("XY Position Data: all slices");
grid minor

%% Pseudo position data - For Unity Model
figure
N = length(x3);
slice3 = (1*randn(N,1) + 0);
scatter3(xnorm3,ynorm3,slice3,'red')
xlabel("X"),ylabel("Y"),zlabel("Z");
title("Pseudo Position Data: middle slice");
grid minor

initPos = [xnorm3, ynorm3, slice3];
%disp(initPos);
writematrix(initPos, "initPos.csv");



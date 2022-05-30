clc, clear all, close all

x = linspace(-20,20,100);% x distance
s = 1;% sigma
y = linspace(-20,20,100);% y distance
[xx, yy] = meshgrid(x,y);
rr = (xx.^2 + yy.^2).^(0.5);% r (total distance from cell)
ff0 = (rr.^2/s^4 - 1/s^2).*exp(-rr.^2/(2*s^2));% LoG


c = 15;% shift value
nrr = rr-c;% shifted r
nss = s*1.2;% adjust sigma value
ffn = 1/3*(nrr/nss^2).*exp(-(nrr.^2)/(2*nss^2));% DoG


% c = -5;% shift value
% nrr = rr - c;% shifted r
% nss = s/10;% adjust sigma value
% ffn_ = (-nrr/nss^2).*exp(-(nrr.^2)/(2*nss^2));% DoG


figure
hold on
surf(xx,yy,ff0+ffn)
%surf(xx,yy,ffn)
% surf(xx,yy,ffn_)
axis tight
shading interp
colorbar

xlabel("X")
ylabel("Y")
zlabel("Force")
title("Two Neighbourhood Force Field: 2D Kernel Visualisation")
grid minor

% ax = gca;
% ax.XAxisLocation = 'origin';
% ax.YAxisLocation = 'origin';
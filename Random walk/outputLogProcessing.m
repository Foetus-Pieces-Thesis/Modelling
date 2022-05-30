clear all, close all, clc;
%% Analysing the time series of our stem cell motility model

X = csvread('PositionLog.csv');% cell tracking data; 50 cells; 2995 frames
Ts = 0.01;% Sampling period?
M = 1;% Downsampling Factor
id = 1;% Cell no you wish to analyse;

x_1 = X(1:M:end,3*id-2);% x position of cell no. 1 
y_1 = X(1:M:end,3*id-1);% y position of cell no. 1
z_1 = X(1:M:end,3*id);% z position of cell no. 1

xnorm_1 = zscore(x_1)% normalized data
ynorm_1 = zscore(y_1)
znorm_1 = zscore(z_1)


%% Plot path in 3D space
figure
subplot(1,2,1)
plot3(x_1,y_1,z_1)
xlabel("X"),ylabel("Y"),zlabel("Z")
title("3D Cell Path: cell "+id)
grid minor

subplot(1,2,2)
plot3(xnorm_1,ynorm_1,znorm_1)
xlabel("X"),ylabel("Y"),zlabel("Z")
title("Normalized 3D Cell Path: cell "+id)
grid minor

%% Plot Autocorrelation function of data
figure

% x positions
subplot(3,2,1)
[Rx lags] = xcorr(x_1,'unbiased');
stem(lags,Rx,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of posX: cell "+id)
grid minor
subplot(3,2,2)
[Rx lags] = xcorr(xnorm_1,'unbiased');
stem(lags,Rx,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of Normalized posX: cell "+id)
grid minor

% y positions
subplot(3,2,3)
[Ry lags] = xcorr(y_1,'unbiased');
stem(lags,Ry,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of posY: cell "+id)
grid minor
subplot(3,2,4)
[Ry lags] = xcorr(ynorm_1,'unbiased');
stem(lags,Ry,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of Normalized posY: cell "+id)
grid minor

%z positions
subplot(3,2,5)
[Rz lags] = xcorr(z_1,'unbiased');
stem(lags,Rz,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of posZ: cell "+id)
grid minor
subplot(3,2,6)
[Rz lags] = xcorr(znorm_1,'unbiased');
stem(lags,Rz,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of Normalized posZ: cell "+id)
grid minor

%% Auto Regressive Modelling

% Yule Walker Coefficient Analysis
figure

% x positions
subplot(3,2,1)
for p=1:10
    [arcoefs, E, K] = aryule(x_1,p);
    plot(arcoefs(1:p+1),"DisplayName",num2str(p))
    hold on
end
axis([1 10.5 -2 1])
xlabel("AR Order p")
ylabel("Coefficients")
title("Yule Walker coefficients for posX (cell "+id+"): p=1-10")
grid minor
legend('show')
hold off
subplot(3,2,2)
for p=1:10
    [arcoefs, E, K] = aryule(x_1,p);
    plot(arcoefs(1:p+1),"DisplayName",num2str(p))
    hold on
end
axis([1 10.5 -2 1])
xlabel("AR Order p")
ylabel("Coefficients")
title("Yule Walker coefficients for Normalized posX (cell "+id+"): p=1-10")
grid minor
legend('show')
hold off


% y positions
subplot(3,2,3)
for p=1:10
    [arcoefs, E, K] = aryule(y_1,p);
    plot(arcoefs(1:p+1),"DisplayName",num2str(p))
    hold on
end
axis([1 10.5 -2 1])
xlabel("AR Order p")
ylabel("Coefficients")
title("Yule Walker coefficients for posY (cell "+id+"): p=1-10")
grid minor
legend('show')
hold off
subplot(3,2,4)
for p=1:10
    [arcoefs, E, K] = aryule(ynorm_1,p);
    plot(arcoefs(1:p+1),"DisplayName",num2str(p))
    hold on
end
axis([1 10.5 -2 1])
xlabel("AR Order p")
ylabel("Coefficients")
title("Yule Walker coefficients for Normalized posY (cell "+id+"): p=1-10")
grid minor
legend('show')
hold off


% z positions
subplot(3,2,5)
for p=1:10
    [arcoefs, E, K] = aryule(z_1,p);
    plot(arcoefs(1:p+1),"DisplayName",num2str(p))
    hold on
end
axis([1 10.5 -2 1])
xlabel("AR Order p")
ylabel("Coefficients")
title("Yule Walker coefficients for posZ (cell "+id+"): p=1-10")
grid minor
legend('show')
hold off
subplot(3,2,6)
for p=1:10
    [arcoefs, E, K] = aryule(z_1,p);
    plot(arcoefs(1:p+1),"DisplayName",num2str(p))
    hold on
end
axis([1 10.5 -2 1])
xlabel("AR Order p")
ylabel("Coefficients")
title("Yule Walker coefficients for Normalized posZ (cell "+id+"): p=1-10")
grid minor
legend('show')
hold off

%% Partial Autocorrelation Function 
N = length(x_1);
conf = sqrt(2)*erfinv(0.95)/sqrt(N);

figure
% x positions
subplot(3,2,1)
[arcoefs, E, K] = aryule(x_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for posX: cell "+id)
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off
grid minor

subplot(3,2,2)
[arcoefs, E, K] = aryule(xnorm_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for Normalized posX: cell "+id)
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off
grid minor


% y positions
subplot(3,2,3)
[arcoefs, E, K] = aryule(y_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for posZ: cell "+id)
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off
grid minor

subplot(3,2,4)
[arcoefs, E, K] = aryule(ynorm_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for Normalized posY: cell "+id)
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off
grid minor

%z positions
subplot(3,2,5)
[arcoefs, E, K] = aryule(z_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for posZ: cell 1")
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off
grid minor

subplot(3,2,6)
[arcoefs, E, K] = aryule(znorm_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for Normalized posZ: cell "+id)
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off 
grid minor

%% MDL, ACI, ACI_c, Cumulative Error Analysis
for p=1:10

    [ar, E, K] = aryule(x_1,p);
    x_ar(:,p) = filter(E^(1/2),ar,x_1);   
end

err = zeros(N,10);
err(1,:) = (x_ar(1,:)-x_1(1)).^2;

for i=2:N
    err(i,:) = err(i-1,:) + (x_ar(i,:)-x_1(i)).^2;
end

MDL = log(err(N,(1:10))) + (1:10)*log(N)/N;
AIC = log(err(N,(1:10))) + (1:10)*2/N;
AIC_c = zeros(1,10);

for p=1:10
    AIC_c(p) = AIC(p) + (2*p)*(p+1)/(N-p-1);
end

figure
plot(MDL)
hold on
plot(AIC)
plot(AIC_c)
plot(log(err(N,:)))
axis([0 10 -inf inf])
xlabel("Model Order p")
ylabel("Magnitude")
title("AR(1) Model: MDL, AIC and AIC_{c} for posX")
legend("MDL","AIC","AIC_{c}","Cumulative Error Squared")
grid minor
hold off


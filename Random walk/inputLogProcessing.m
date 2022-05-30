%% Import .csv File
clc, close all, clear all, format compact
track_cells = csvread("C1-movie-3_cp_masks_notdo3d-Coordinates.csv");

%% Analysis
clc, clearvars -except track_cells

id = 1;% cell number to analyse (out of 2)

figure

ii=1;
for M = [1,5,20]% Downsampling factor
    
    if(id==2)
        M = 2*M;% values are spaced in this column
    end


    x_1 = track_cells(1:M:end,3*id-1);% x position of cell id
    y_1 = track_cells(1:M:end,3*id);% y position of cell id
    
    xnorm_1 = zscore(x_1);% normalised data
    ynorm_1 = zscore(y_1);

    N = length(xnorm_1);
    t = linspace(0,100,N);

%     subplot(1,3,ii)
%     plot(x1,y1,'b');% Plot 2D path
%     xlabel("X"),ylabel("Y")
%     title("2D path of cell 1: M="+M)
%     grid minor
    
    subplot(1,3,ii)
    plot3(xnorm_1,ynorm_1,t,'b');% Plot 2D path through time
    xlabel("X"),ylabel("Y"),zlabel("t")
    title("Normalised 2D path of cell "+id+": M="+M)
    grid minor

    ii = ii+1;
end

%% Deep Analysis

id = 1;

M = 1
if(id==2)
    M = 2*M;% values are spaced in this column
end

x_1 = track_cells(1:M:end,3*id-1);% x position of cell 1
y_1 = track_cells(1:M:end,3*id);% y position of cell 1

xnorm_1 = zscore(x_1);% normalised data
ynorm_1 = zscore(y_1);



%% Plot Autocorrelation function of data
figure

% x positions
subplot(2,2,1)
[Rx lags] = xcorr(x_1,'unbiased');
stem(lags,Rx,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of posX: cell "+id)
grid minor
subplot(2,2,2)
[Rx lags] = xcorr(xnorm_1,'unbiased');
stem(lags,Rx,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of Normalized posX: cell "+id)
grid minor

% y positions
subplot(2,2,3)
[Ry lags] = xcorr(y_1,'unbiased');
stem(lags,Ry,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of posY: cell "+id)
grid minor
subplot(2,2,4)
[Ry lags] = xcorr(ynorm_1,'unbiased');
stem(lags,Ry,'Marker','.')
xlabel("Correlation Lag (\tau)")
ylabel("Correlation")
title("ACF of Normalized posY: cell "+id)
grid minor


%% Auto Regressive Modelling

% Yule Walker Coefficient Analysis
figure

% x positions
subplot(2,2,1)
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
subplot(2,2,2)
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
subplot(2,2,3)
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
subplot(2,2,4)
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

%% Partial Autocorrelation Function 
N = length(x_1);
conf = sqrt(2)*erfinv(0.95)/sqrt(N);

figure

% x positions
subplot(2,2,1)
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

subplot(2,2,2)
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
subplot(2,2,3)
[arcoefs, E, K] = aryule(y_1,10);
stem(-K,"filled");
xlabel("Correlation Lag")
ylabel("Correlation")
title("Partial ACF for posY: cell "+id)
xlim([1 10])
ylim([-1 1])
hold on
plot(xlim,[1 1]'*[-conf conf],'r','LineStyle','--')
hold off
grid minor

subplot(2,2,4)
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


%% MDL, ACI, ACI_c, Cumulative Error Analysis
for p=1:10

    [ar, E, K] = aryule(y_1,p);
    x_ar(:,p) = filter(E^(1/2),ar,y_1);   
end

err = zeros(N,10);
err(1,:) = (x_ar(1,:)-y_1(1)).^2;

for i=2:N
    err(i,:) = err(i-1,:) + (x_ar(i,:)-y_1(i)).^2;
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
title("MDL, AIC and AIC_{c} for posY: cell "+id)
legend("MDL","AIC","AIC_{c}","Cumulative Error Squared")
grid minor
hold off
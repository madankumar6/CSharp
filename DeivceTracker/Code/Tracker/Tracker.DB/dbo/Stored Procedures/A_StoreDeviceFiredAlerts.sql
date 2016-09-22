CREATE PROCEDURE [dbo].[A_StoreDeviceFiredAlerts]
(
	@DeviceAlertId INT,
	@AlertType INT,
	@Eval NVARCHAR(150),
	@Status INT,           
	@ProcessorId INT = NULL,
	@AlertDataFromService DeviceAlertDataType_V1 READONLY
)
AS
BEGIN
	
	INSERT INTO [DeviceFiredAlerts]
           ([DeviceAlertId]
           ,[AlertType]
           ,[Eval]
           ,[Status]
           ,[DeviceId]
           ,[IMEI]
           ,[CommandType]
           ,[StatusCode]
           ,[Latitude]
           ,[Longitude]
           ,[Speed]
           ,[Direction]
           ,[Altitude]
           ,[Mileage]
           ,[Odometer]
           ,[OnBattery]
           ,[OnIgnition]
           ,[OnAc]
           ,[OnGps]
           ,[OnAcc]
           ,[OilNElectricConected]
           ,[OnSOS]
           ,[OnLowBattery]
           ,[OnPowerCut]
           ,[OnShock]
           ,[OnCharge]
           ,[OnDefence]
           ,[VoltageLevel]
           ,[SignalStrengthLevel]
           ,[AlarmType]
           ,[TrackerIp]
           ,[DeviceDataTime]
           ,[TrackerConnectedTime]
           ,[TrackerDataActionTime]
           ,[TrackerDataParsedTime]
           ,[ActionTime])
           
    SELECT TOP 1
		@DeviceAlertId,
		@AlertType,
		@Eval,
		@Status,
		
		DeviceId, IMEI, CommandType, StatusCode, Latitude, Longitude, Speed, Direction, Altitude,
		Mileage, Odometer, OnBattery, OnIgnition, OnAc, OnGps, OnAcc, OilNElectricConected, OnSOS,
		OnLowBattery, OnPowerCut, OnShock, OnCharge, OnDefence, VoltageLevel, SignalStrengthLevel,
		AlarmType, TrackerIp, DeviceDataTime, TrackerConnectedTime, TrackerDataActionTime, TrackerDataParsedTime, ActionTime
    FROM @AlertDataFromService
   	
END
CREATE PROCEDURE [dbo].[T_StoreDeviceStatus]
(
	@DeviceId NVARCHAR(50) = NULL,
	@IsOnline BIT = NULL
)
AS
BEGIN

	DECLARE @ActionTime DATETIME;
	SET @ActionTime = GETUTCDATE();
	
	UPDATE DeviceData
	SET
	  IsOnline = @IsOnline
	WHERE
		DeviceId = @DeviceId
	
	INSERT INTO DeviceData_History(
		DeviceId,
		IMEI,
		CommandType,
		StatusCode,
		Latitude,
		Longitude,
		Speed,
		Direction,
		Altitude,
		Mileage,
		ValidData,
		FullAddress,
		PayLoad,
		UnParsedPayload,
		Distance,
		Odometer,
		OnBattery,
		OnIgnition,
		OnAc,
		OnGps,
		OnAcc,
		OilNElectricConected,
		OnSOS,
		OnLowBattery,
		OnPowerCut,
		OnShock,
		OnCharge,
		OnDefence,
		VoltageLevel,
		SignalStrengthLevel,
		AlarmType,
		UnKnown,
		GeozoneIndex,
		GeozoneID,
		TrackerIp,
		DeviceDataTime,
		TrackerConnectedTime,
		TrackerDataActionTime,
		TrackerDataParsedTime,
		ActionTime,
		IsOnline
	)
	SELECT 
		DeviceId,
		IMEI,
		CommandType,
		StatusCode,
		Latitude,
		Longitude,
		Speed,
		Direction,
		Altitude,
		Mileage,
		ValidData,
		FullAddress,
		PayLoad,
		UnParsedPayload,
		Distance,
		Odometer,
		OnBattery,
		OnIgnition,
		OnAc,
		OnGps,
		OnAcc,
		OilNElectricConected,
		OnSOS,
		OnLowBattery,
		OnPowerCut,
		OnShock,
		OnCharge,
		OnDefence,
		VoltageLevel,
		SignalStrengthLevel,
		AlarmType,
		UnKnown,
		GeozoneIndex,
		GeozoneID,
		TrackerIp,
		DeviceDataTime,
		TrackerConnectedTime,
		TrackerDataActionTime,
		TrackerDataParsedTime,
		ActionTime,
		@IsOnline
	FROM DeviceData
	WHERE DeviceId = @DeviceId
	
END
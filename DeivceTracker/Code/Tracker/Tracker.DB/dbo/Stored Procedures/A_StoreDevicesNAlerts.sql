CREATE PROCEDURE [dbo].[A_StoreDevicesNAlerts]
(
	@ProcessorId INT = NULL,
	@AlertDataFromService DeviceAlertDataType_V1 READONLY
)
AS
BEGIN
	
	UPDATE DeviceAlertData_Prev
	SET
		DeviceId		=	ADFS.DeviceId	,
		IMEI			=	ADFS.IMEI	,
		CommandType		=	ADFS.CommandType	,
		StatusCode		=	ADFS.StatusCode	,
		Latitude		=	ADFS.Latitude	,
		Longitude		=	ADFS.Longitude	,
		Speed			=	ADFS.Speed	,
		Direction		=	ADFS.Direction	,
		Altitude		=	ADFS.Altitude	,
		Mileage			=	ADFS.Mileage	,
		Odometer		=	ADFS.Odometer	,
		OnBattery		=	ADFS.OnBattery	,
		OnIgnition		=	ADFS.OnIgnition	,
		OnAc			=	ADFS.OnAc	,
		OnGps			=	ADFS.OnGps	,
		OnAcc			=	ADFS.OnAcc	,
		OilNElectricConected	=	ADFS.OilNElectricConected	,
		OnSOS			=	ADFS.OnSOS	,
		OnLowBattery	=	ADFS.OnLowBattery	,
		OnPowerCut		=	ADFS.OnPowerCut	,
		OnShock			=	ADFS.OnShock	,
		OnCharge		=	ADFS.OnCharge	,
		OnDefence		=	ADFS.OnDefence	,
		VoltageLevel	=	ADFS.VoltageLevel	,
		SignalStrengthLevel	=	ADFS.SignalStrengthLevel	,
		AlarmType		=	ADFS.AlarmType	,
		TrackerIp		=	ADFS.TrackerIp	,
		DeviceDataTime	=	ADFS.DeviceDataTime	,
		TrackerConnectedTime	=	ADFS.TrackerConnectedTime	,
		TrackerDataActionTime	=	ADFS.TrackerDataActionTime	,
		TrackerDataParsedTime	=	ADFS.TrackerDataParsedTime	,
		ActionTime		=	ADFS.ActionTime	
	FROM
		DeviceAlertData_Prev DADPrev
	INNER JOIN
		@AlertDataFromService ADFS
	ON DADPrev.DeviceId = ADFS.DeviceId
	
	;WITH CTE AS (
			SELECT row_number() OVER (PARTITION BY DeviceId ORDER BY ActionTime) AS [R],
			*
			FROM @AlertDataFromService
	)
	INSERT DeviceAlertData_Prev(
		DeviceId, IMEI, CommandType, StatusCode, Latitude, Longitude, Speed, Direction, Altitude,
		Mileage, Odometer, OnBattery, OnIgnition, OnAc, OnGps, OnAcc, OilNElectricConected, OnSOS,
		OnLowBattery, OnPowerCut, OnShock, OnCharge, OnDefence, VoltageLevel, SignalStrengthLevel,
		AlarmType, TrackerIp, DeviceDataTime, TrackerConnectedTime, TrackerDataActionTime, TrackerDataParsedTime, ActionTime)
	SELECT 
		DeviceId, IMEI, CommandType, StatusCode, Latitude, Longitude, Speed, Direction, Altitude,
		Mileage, Odometer, OnBattery, OnIgnition, OnAc, OnGps, OnAcc, OilNElectricConected, OnSOS,
		OnLowBattery, OnPowerCut, OnShock, OnCharge, OnDefence, VoltageLevel, SignalStrengthLevel,
		AlarmType, TrackerIp, DeviceDataTime, TrackerConnectedTime, TrackerDataActionTime, TrackerDataParsedTime, ActionTime
	FROM 
		CTE
	WHERE DeviceId NOT IN (SELECT DeviceId FROM DeviceAlertData_Prev)
	AND
	CTE.R = 1;
	
	
	UPDATE DeviceAlertLog
	SET
		IsSent			= ADFS.IsSent,
		SentTime		= ADFS.SentTime,
		ConditionState	= ADFS.ConditionState,
		ConditionStateTime=	ADFS.ConditionStateTime
	FROM
		DeviceAlertLog DAL
	INNER JOIN
		@AlertDataFromService ADFS
	ON DAL.DeviceAlertId = ADFS.DeviceAlertId
	AND DAL.DeviceId = ADFS.DeviceId
	
	INSERT INTO DeviceAlertLog
	(
		DeviceAlertId,
		DeviceId,
		IsSent,
		SentTime,
		ConditionState,
		ConditionStateTime
	)
	SELECT 
		ADFS.DeviceAlertId,
		ADFS.DeviceId,
		ADFS.IsSent,
		ADFS.SentTime,
		ADFS.ConditionState,
		ADFS.ConditionStateTime
	FROM
		@AlertDataFromService ADFS
	LEFT JOIN
		DeviceAlertLog DAL	
	ON DAL.DeviceAlertId = ADFS.DeviceAlertId
	AND DAL.DeviceId = ADFS.DeviceId
	WHERE DAL.DeviceAlertId IS NULL -- OUTER JOIN	
	
	
	
	DELETE FROM DeviceAlertData
	WHERE
		ProcessorId = @ProcessorId
	
	--update DeviceAlertData
	--set
	--	ProcessorId = null
	--WHERE
	--	ProcessorId = @ProcessorId
END
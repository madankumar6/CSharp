CREATE PROCEDURE [dbo].[A_GetDevicesNAlerts]
(
	@ProcessorId INT
)
AS
BEGIN
	;WITH CTE AS (
		SELECT top 300 row_number() OVER (PARTITION BY DeviceId ORDER BY ActionTime) AS [R],
		*
		FROM DeviceAlertData
		WHERE
			DeviceId NOT IN (SELECT DISTINCT DeviceId FROM DeviceAlertData WHERE ProcessorId IS NOT NULL)
		ORDER BY ActionTime
	)	
	UPDATE CTE
	SET ProcessorId = @ProcessorId
	WHERE
		CTE.R = 1
	
	SELECT 
		DAD.DeviceId,
		DAD.Speed,
		
		DAD.CommandType,
		DAD.StatusCode,
		DAD.Latitude,
		DAD.Longitude,
		DAD.Speed,
		DAD.Direction,
		DAD.Altitude,
		DAD.Mileage,
		DAD.Odometer,
		DAD.OnBattery,
		DAD.OnIgnition,
		DAD.OnAc,
		DAD.OnGps,
		DAD.OnAcc,
		DAD.OilNElectricConected,
		DAD.OnSOS,
		DAD.OnLowBattery,
		DAD.OnPowerCut,
		DAD.OnShock,
		DAD.OnCharge,
		DAD.OnDefence,
		DAD.VoltageLevel,
		DAD.SignalStrengthLevel,
		DAD.AlarmType,
		DAD.TrackerIp,
		COALESCE(DAD.DeviceDataTime, GETUTCDATE()) DeviceDataTime,
		DAD.TrackerConnectedTime,
		DAD.TrackerDataActionTime,
		DAD.TrackerDataParsedTime,
		DAD.ActionTime,
		
		DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETUTCDATE()), 0), GETUTCDATE()) CurrentDate,
		DATEDIFF(MINUTE, DATEADD(DAY, DATEDIFF(DAY, 0, GETUTCDATE()), 0), GETUTCDATE()) CurrentTime,
		
		COALESCE(DADPrev.CommandType, DAD.CommandType) AS PrevCommandType,
		COALESCE(DADPrev.StatusCode, DAD.StatusCode) AS PrevStatusCode,
		COALESCE(DADPrev.Latitude, DAD.Latitude) AS PrevLatitude,
		COALESCE(DADPrev.Longitude, DAD.Longitude) AS PrevLongitude,
		COALESCE(DADPrev.Speed, DAD.Speed) AS PrevSpeed,
		COALESCE(DADPrev.Direction, DAD.Direction) AS PrevDirection,
		COALESCE(DADPrev.Altitude, DAD.Altitude) AS PrevAltitude,
		COALESCE(DADPrev.Mileage, DAD.Mileage) AS PrevMileage,
		COALESCE(DADPrev.Odometer, DAD.Odometer) AS PrevOdometer,
		COALESCE(DADPrev.OnBattery, DAD.OnBattery) AS PrevOnBattery,
		COALESCE(DADPrev.OnIgnition, DAD.OnIgnition) AS PrevOnIgnition,
		COALESCE(DADPrev.OnAc, DAD.OnAc) AS PrevOnAc,
		COALESCE(DADPrev.OnGps, DAD.OnGps) AS PrevOnGps,
		COALESCE(DADPrev.OnAcc, DAD.OnAcc) AS PrevOnAcc,
		COALESCE(DADPrev.OilNElectricConected, DAD.OilNElectricConected) AS PrevOilNElectricConected,
		COALESCE(DADPrev.OnSOS, DAD.OnSOS) AS PrevOnSOS,
		COALESCE(DADPrev.OnLowBattery, DAD.OnLowBattery) AS PrevOnLowBattery,
		COALESCE(DADPrev.OnPowerCut, DAD.OnPowerCut) AS PrevOnPowerCut,
		COALESCE(DADPrev.OnShock, DAD.OnShock) AS PrevOnShock,
		COALESCE(DADPrev.OnCharge, DAD.OnCharge) AS PrevOnCharge,
		COALESCE(DADPrev.OnDefence, DAD.OnDefence) AS PrevOnDefence,
		COALESCE(DADPrev.VoltageLevel, DAD.VoltageLevel) AS PrevVoltageLevel,
		COALESCE(DADPrev.SignalStrengthLevel, DAD.SignalStrengthLevel) AS PrevSignalStrengthLevel,
		COALESCE(DADPrev.AlarmType, DAD.AlarmType) AS PrevAlarmType,
		COALESCE(DADPrev.TrackerIp, DAD.TrackerIp) AS PrevTrackerIp,
		COALESCE(DADPrev.DeviceDataTime, DAD.DeviceDataTime) AS PrevDeviceDataTime,
		COALESCE(DADPrev.TrackerConnectedTime, DAD.TrackerConnectedTime) AS PrevTrackerConnectedTime,
		COALESCE(DADPrev.TrackerDataActionTime, DAD.TrackerDataActionTime) AS PrevTrackerDataActionTime,
		COALESCE(DADPrev.TrackerDataParsedTime, DAD.TrackerDataParsedTime) AS PrevTrackerDataParsedTime,
		COALESCE(DADPrev.ActionTime, DAD.ActionTime) AS PrevActionTime,
		
		DA.Id DeviceAlertId,
		DA.AlertType,
		DA.Eval,
		DA.IsActive,
		
		COALESCE(DAL.IsSent, 0) IsSent,
		COALESCE(DAL.SentTime, GETUTCDATE()) SentTime,
		
		CAST(COALESCE(DAL.ConditionState, 0) AS BIT) ConditionState,
		COALESCE(DAL.ConditionStateTime, GETUTCDATE()) ConditionStateTime
	FROM
		DeviceAlertData DAD
	LEFT JOIN
		DeviceAlertData_Prev DADPrev
	ON DAD.DeviceId = DADPrev.DeviceId
	LEFT JOIN
		AlertDevices AD
	ON DAD.DeviceId = AD.DeviceId
	INNER JOIN
		DeviceAlerts DA
	ON AD.DeviceAlertId = DA.Id
	LEFT JOIN
		DeviceAlertLog DAL
	ON AD.DeviceAlertId = DAL.DeviceAlertiD AND AD.DeviceId = DAL.DeviceId
	WHERE
		DA.IsActive = 1
	AND
		ProcessorId = @ProcessorId
	--AND
	--	DAD.DeviceId='0003030303030303' and DA.AlertType=3
	ORDER BY DAD.ActionTime, DAD.DeviceId, DA.AlertType
	
END
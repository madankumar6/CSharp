CREATE PROCEDURE [dbo].[A_GetFiredAlerts]
(
	@ProcessorId INT = NULL
)
AS
BEGIN

	IF NOT EXISTS(SELECT TOP 1 1 FROM DeviceFiredAlerts WHERE ProcessorId = @ProcessorId)
	BEGIN
	
		;WITH CTE AS (
			SELECT top 300
			*
			FROM DeviceFiredAlerts
			WHERE COALESCE(ProcessorId, 0) = 0 
			ORDER BY ActionTime
		)	
		UPDATE CTE
		SET ProcessorId = @ProcessorId
	END
	
	SELECT --TOP 100
		DFA.DeviceAlertId,
		DA.Name,
		DA.DescriptionText,
		DFA.AlertType,
		DFA.Eval,
		DFA.Status,
		DFA.DeviceId,
		DFA.IMEI,
		DFA.CommandType,
		DFA.StatusCode,
		DFA.Latitude,
		DFA.Longitude,
		DFA.Speed,
		DFA.Direction,
		DFA.Altitude,
		DFA.Mileage,
		DFA.Odometer,
		DFA.OnBattery,
		DFA.OnIgnition,
		DFA.OnAc,
		DFA.OnGps,
		DFA.OnAcc,
		DFA.OilNElectricConected,
		DFA.OnSOS,
		DFA.OnLowBattery,
		DFA.OnPowerCut,
		DFA.OnShock,
		DFA.OnCharge,
		DFA.OnDefence,
		DFA.VoltageLevel,
		DFA.SignalStrengthLevel,
		DFA.AlarmType,
		DFA.TrackerIp,
		DFA.DeviceDataTime,
		DFA.TrackerConnectedTime,
		DFA.TrackerDataActionTime,
		DFA.TrackerDataParsedTime,
		DFA.ActionTime,
		DFA.ProcessorId
	FROM
		DeviceFiredAlerts DFA
	INNER JOIN
		DeviceAlerts DA
	ON	DFA.DeviceAlertId = DA.Id
	WHERE
		ProcessorId = @ProcessorId

END
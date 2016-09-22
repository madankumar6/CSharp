CREATE PROCEDURE [dbo].[T_Sub_StoreDeviceCalcData]
(
	@DeviceId NVARCHAR(50) = NULL,
	@IMEI NVARCHAR(50) = NULL,
	@CommandType NVARCHAR(50) = NULL,
	@StatusCode NVARCHAR(50) = NULL,
	@Latitude NVARCHAR(50) = NULL,
	@Longitude NVARCHAR(50) = NULL,
	@Speed NVARCHAR(50) = NULL,
	@Direction NVARCHAR(50) = NULL,
	@Altitude NVARCHAR(50) = NULL,
	@Mileage NVARCHAR(50) = NULL,
	@ValidData NVARCHAR(50) = NULL,
	@FullAddress NVARCHAR(4000) = NULL,
	@PayLoad TEXT = NULL,
	@UnParsedPayload TEXT = NULL,
	@Distance NVARCHAR(50) = NULL,
	@Odometer INT = NULL,
	@OnBattery INT = NULL,
	@OnIgnition INT = NULL,
	@OnAc INT = NULL,
	@OnGps INT = NULL,
	@UnKnown NVARCHAR(500) = NULL,
	@GeozoneIndex NVARCHAR(50) = NULL,
	@GeozoneID NVARCHAR(50) = NULL,
	@TrackerIp NVARCHAR(50) = NULL,

	@DeviceDataTime DATETIME = NULL,
	@TrackerConnectedTime DATETIME = NULL,
	@TrackerDataActionTime DATETIME  = NULL,
	@TrackerDataParsedTime DATETIME = NULL,
	
	@OnAcc INT = NULL,
    @OilNElectricConected INT = NULL,
    @OnSOS INT = NULL,
    @OnLowBattery INT = NULL,
    @OnPowerCut INT = NULL,
    @OnShock INT = NULL,
    @OnCharge INT = NULL,
    @OnDefence INT = NULL,
    @VoltageLevel INT = NULL,
    @SignalStrengthLevel INT = NULL,
    @AlarmType INT = NULL
)
AS
BEGIN

	DECLARE @ActionTime DATETIME;
	SET @ActionTime = GETUTCDATE();
	
	IF EXISTS(SELECT 1 FROM [DeviceCalcData] WHERE DeviceId = @DeviceId)
	BEGIN
		UPDATE [DeviceCalcData]
		SET
			Odometer = Odometer + (
				CASE WHEN ISNUMERIC(@Odometer) = 1 THEN CAST(@Odometer AS INT) ELSE 0 END
			),
		   [ActionTime] = @ActionTime
		WHERE
			DeviceId = @DeviceId
	END
	ELSE
	BEGIN
		INSERT INTO [DeviceCalcData](
				DeviceId
			   ,Odometer
			   ,ActionTime
			   )
		 VALUES
			   (
			   @DeviceId
			   ,CASE WHEN ISNUMERIC(@Odometer) = 1 THEN CAST(@Odometer AS INT) ELSE 0 END
			   ,@ActionTime
			   )
	END
	   
END
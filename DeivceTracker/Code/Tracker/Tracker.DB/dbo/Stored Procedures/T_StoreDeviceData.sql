CREATE PROCEDURE [dbo].[T_StoreDeviceData]
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

	SET @PayLoad = @PayLoad;
	
	DECLARE @ActionTime DATETIME;
	SET @ActionTime = GETUTCDATE();
	
	IF EXISTS(SELECT 1 FROM DeviceData WHERE DeviceId = @DeviceId)
	BEGIN
		UPDATE [DeviceData]
		SET
		   [IMEI] = @IMEI
		  ,[CommandType] = @CommandType
		  ,[StatusCode] = @StatusCode
		  ,[Latitude] = @Latitude
		  ,[Longitude] = @Longitude
		  ,[Speed] = @Speed
		  ,[Direction] = @Direction
		  ,[Altitude] = @Altitude
		  ,[Mileage] = @Mileage
		  ,[ValidData] = @ValidData
		  ,[FullAddress] = @FullAddress
		  ,[PayLoad] = @PayLoad
		  ,[UnParsedPayload] = @UnParsedPayload
		  ,[Distance] = @Distance
		  ,[Odometer] = @Odometer
		  ,[OnBattery] = @OnBattery
		  ,[OnIgnition] = @OnIgnition
		  ,[OnAc] = @OnAc
		  ,[OnGps] = @OnGps
		  
		  ,[OnAcc] = @OnAcc
		  ,[OilNElectricConected] = @OilNElectricConected
		  ,[OnSOS] = @OnSOS
		  ,[OnLowBattery] = @OnLowBattery
		  ,[OnPowerCut] = @OnPowerCut
		  ,[OnShock] = @OnShock
		  ,[OnCharge] = @OnCharge
		  ,[OnDefence] = @OnDefence
		  ,[VoltageLevel] = @VoltageLevel
		  ,[SignalStrengthLevel] = @SignalStrengthLevel
		  ,[AlarmType] = @AlarmType
        
		  ,[UnKnown] = @UnKnown
		  ,[GeozoneIndex] = @GeozoneIndex
		  ,[GeozoneID] = @GeozoneID
		  ,[TrackerIp] = @TrackerIp
		  ,[DeviceDataTime] = @DeviceDataTime
		  ,[TrackerConnectedTime] = @TrackerConnectedTime
		  ,[TrackerDataActionTime] = @TrackerDataActionTime
		  ,[TrackerDataParsedTime] = @TrackerDataParsedTime
		  ,[ActionTime] = @ActionTime,
			IsOnline = 1
		WHERE
			DeviceId = @DeviceId
		--AND
		--	CommandType = @CommandType	
	END
	ELSE
	BEGIN
		INSERT INTO DeviceData(
				DeviceId
			   ,IMEI
			   ,CommandType
			   ,StatusCode
			   ,Latitude
			   ,Longitude
			   ,Speed
			   ,Direction
			   ,Altitude
			   ,Mileage
			   ,ValidData
			   ,FullAddress
			   ,PayLoad
			   ,UnParsedPayload
			   ,Distance
			   ,Odometer
			   ,OnBattery
			   ,OnIgnition
			   ,OnAc
			   ,OnGps
			   ,UnKnown
			   ,GeozoneIndex
			   ,GeozoneID
			   ,TrackerIp
			   ,DeviceDataTime
			   ,TrackerConnectedTime
			   ,TrackerDataActionTime
			   ,TrackerDataParsedTime
			   ,ActionTime
			   
			   ,OnAcc
			   ,OilNElectricConected
			   ,OnSOS
			   ,OnLowBattery
			   ,OnPowerCut
			   ,OnShock
			   ,OnCharge
			   ,OnDefence
			   ,VoltageLevel
			   ,SignalStrengthLevel
			   ,AlarmType,
			   IsOnline
			   )
		 VALUES
			   (
			   @DeviceId
			   ,@IMEI
			   ,@CommandType
			   ,@StatusCode
			   ,@Latitude
			   ,@Longitude
			   ,@Speed
			   ,@Direction
			   ,@Altitude
			   ,@Mileage
			   ,@ValidData
			   ,@FullAddress
			   ,@PayLoad
			   ,@UnParsedPayload
			   ,@Distance
			   ,@Odometer
			   ,@OnBattery
			   ,@OnIgnition
			   ,@OnAc
			   ,@OnGps
			   ,@UnKnown
			   ,@GeozoneIndex
			   ,@GeozoneID
			   ,@TrackerIp
			   ,@DeviceDataTime
			   ,@TrackerConnectedTime
			   ,@TrackerDataActionTime
			   ,@TrackerDataParsedTime
			   ,GETUTCDATE()
			   
			   ,@OnAcc
			   ,@OilNElectricConected
			   ,@OnSOS
			   ,@OnLowBattery
			   ,@OnPowerCut
			   ,@OnShock
			   ,@OnCharge
			   ,@OnDefence
			   ,@VoltageLevel
			   ,@SignalStrengthLevel
			   ,@AlarmType,
			   1
			   )
	END
	   
	DECLARE @CommandValue NVARCHAR(150);
	SET @CommandValue = NULL;
	
	
	INSERT INTO DeviceData_History(
				DeviceId
			   ,IMEI
			   ,CommandType
			   ,StatusCode
			   ,Latitude
			   ,Longitude
			   ,Speed
			   ,Direction
			   ,Altitude
			   ,Mileage
			   ,ValidData
			   ,FullAddress
			   ,PayLoad
			   ,UnParsedPayload
			   ,Distance
			   ,Odometer
			   ,OnBattery
			   ,OnIgnition
			   ,OnAc
			   ,OnGps
			   ,UnKnown
			   ,GeozoneIndex
			   ,GeozoneID
			   ,TrackerIp
			   ,DeviceDataTime
			   ,TrackerConnectedTime
			   ,TrackerDataActionTime
			   ,TrackerDataParsedTime
			   ,ActionTime			   
			   
			   ,OnAcc
			   ,OilNElectricConected
			   ,OnSOS
			   ,OnLowBattery
			   ,OnPowerCut
			   ,OnShock
			   ,OnCharge
			   ,OnDefence
			   ,VoltageLevel
			   ,SignalStrengthLevel
			   ,AlarmType
			   ,IsOnline
			   )
		 VALUES
			   (
			   @DeviceId
			   ,@IMEI
			   ,@CommandType
			   ,@StatusCode
			   ,@Latitude
			   ,@Longitude
			   ,@Speed
			   ,@Direction
			   ,@Altitude
			   ,@Mileage
			   ,@ValidData
			   ,@FullAddress
			   ,@PayLoad
			   ,@UnParsedPayload
			   ,@Distance
			   ,@Odometer
			   ,@OnBattery
			   ,@OnIgnition
			   ,@OnAc
			   ,@OnGps
			   ,@UnKnown
			   ,@GeozoneIndex
			   ,@GeozoneID
			   ,@TrackerIp
			   ,@DeviceDataTime
			   ,@TrackerConnectedTime
			   ,@TrackerDataActionTime
			   ,@TrackerDataParsedTime
			   ,GETUTCDATE()
			   
			   ,@OnAcc
			   ,@OilNElectricConected
			   ,@OnSOS
			   ,@OnLowBattery
			   ,@OnPowerCut
			   ,@OnShock
			   ,@OnCharge
			   ,@OnDefence
			   ,@VoltageLevel
			   ,@SignalStrengthLevel
			   ,@AlarmType
			   ,1
			   )
			   
			   
			   
			   INSERT INTO DeviceAlertData(
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
					TrackerIp,
					DeviceDataTime,
					TrackerConnectedTime,
					TrackerDataActionTime,
					TrackerDataParsedTime,
					ActionTime,
					IsOnline
			   )
		 VALUES
			   (
					@DeviceId,
					@IMEI,
					@CommandType,
					@StatusCode,
					@Latitude,
					@Longitude,
					@Speed,
					@Direction,
					@Altitude,
					@Mileage,
					@Odometer,
					@OnBattery,
					@OnIgnition,
					@OnAc,
					@OnGps,
					@OnAcc,
					@OilNElectricConected,
					@OnSOS,
					@OnLowBattery,
					@OnPowerCut,
					@OnShock,
					@OnCharge,
					@OnDefence,
					@VoltageLevel,
					@SignalStrengthLevel,
					@AlarmType,
					@TrackerIp,
					@DeviceDataTime,
					@TrackerConnectedTime,
					@TrackerDataActionTime,
					@TrackerDataParsedTime,
					GETUTCDATE(),
					1
			   )
			   
			   
		EXEC T_Sub_StoreDeviceCalcData
				@DeviceId,
				@IMEI,
				@CommandType,
				@StatusCode,
				@Latitude,
				@Longitude,
				@Speed,
				@Direction,
				@Altitude,
				@Mileage,
				@ValidData,
				@FullAddress,
				@PayLoad,
				@UnParsedPayload,
				@Distance,
				@Odometer,
				@OnBattery,
				@OnIgnition,
				@OnAc,
				@OnGps,
				@UnKnown,
				@GeozoneIndex,
				@GeozoneID,
				@TrackerIp,

				@DeviceDataTime,
				@TrackerConnectedTime,
				@TrackerDataActionTime,
				@TrackerDataParsedTime,
				
				@OnAcc,
				@OilNElectricConected,
				@OnSOS,
				@OnLowBattery,
				@OnPowerCut,
				@OnShock,
				@OnCharge,
				@OnDefence,
				@VoltageLevel,
				@SignalStrengthLevel,
				@AlarmType
	
		   	           
END
CREATE PROCEDURE [dbo].[T_StoreProtocolServer]
(
	@ProtocolServer NVARCHAR(150) = NULL,
	@Port INT = NULL,
	@Action NVARCHAR(150) = NULL,
	@ActionText NVARCHAR(MAX) = NULL
)
AS
BEGIN

	DECLARE @ActionTime DATETIME;
	SET @ActionTime = GETUTCDATE();

	INSERT INTO [ProtocolServer]
           (
			[ProtocolServer]
           ,[Port]
           ,[Action]
           ,[ActionText]
           ,[ActionTime]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[CreatedBy]
           ,[ModifiedBy])
     VALUES
           (
				@ProtocolServer
			   ,@Port
			   ,@Action
			   ,@ActionText
			   ,@ActionTime
			   ,@ActionTime
			   ,@ActionTime
			   ,'TrackerServer'
			   ,'TrackerServer'
           )
	   	           
END
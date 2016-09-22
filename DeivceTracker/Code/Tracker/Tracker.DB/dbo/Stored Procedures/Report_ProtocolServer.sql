-- =============================================
-- Author:  
-- Create date: 
-- Description: 
-- =============================================
CREATE PROCEDURE [dbo].[Report_ProtocolServer]
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
 -- interfering with SELECT statements.
 SET NOCOUNT ON;

    -- Insert statements for procedure here
 SELECT [Id]
      ,[ProtocolServer]
      ,[Port]
      ,[DevicesConnected]
      ,[Action]
      ,[ActionText]
      ,[ActionTime]
      ,[CreatedDate]
      ,[ModifiedDate]
      ,[CreatedBy]
      ,[ModifiedBy]
  FROM [Tracker].[dbo].[ProtocolServer]
END
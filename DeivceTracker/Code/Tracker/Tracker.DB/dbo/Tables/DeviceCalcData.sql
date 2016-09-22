CREATE TABLE [dbo].[DeviceCalcData] (
    [DeviceId]   NVARCHAR (50) NULL,
    [IMEI]       NVARCHAR (50) NULL,
    [Odometer]   INT           NULL,
    [ActionTime] DATETIME      CONSTRAINT [DF__DeviceCal__Actio__534D60F1] DEFAULT (getutcdate()) NULL
);


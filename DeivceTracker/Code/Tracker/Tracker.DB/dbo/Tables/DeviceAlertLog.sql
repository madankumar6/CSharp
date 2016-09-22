CREATE TABLE [dbo].[DeviceAlertLog] (
    [DeviceAlertId]      INT           NOT NULL,
    [DeviceId]           NVARCHAR (50) NULL,
    [IsSent]             BIT           CONSTRAINT [DF__DeviceAle__IsSen__117F9D94] DEFAULT ((0)) NULL,
    [SentTime]           DATETIME      NULL,
    [ConditionState]     BIT           NULL,
    [ConditionStateTime] DATETIME      NULL
);






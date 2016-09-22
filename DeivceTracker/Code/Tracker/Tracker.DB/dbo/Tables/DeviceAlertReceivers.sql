CREATE TABLE [dbo].[DeviceAlertReceivers] (
    [Id]            INT            IDENTITY (1, 1) NOT NULL,
    [DeviceAlertId] INT            NULL,
    [To]            NVARCHAR (300) NULL,
    [MediumType]    INT            NULL,
    [CreatedDate]   DATETIME       CONSTRAINT [DF_DeviceAlertReceivers_CreatedDate] DEFAULT (getutcdate()) NULL,
    [ModifiedDate]  DATETIME       CONSTRAINT [DF_DeviceAlertReceivers_ModifiedDate] DEFAULT (getutcdate()) NULL,
    CONSTRAINT [PK_DeviceAlertReceivers] PRIMARY KEY CLUSTERED ([Id] ASC)
);


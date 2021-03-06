﻿CREATE TYPE [dbo].[DeviceAlertDataType_V1] AS TABLE (
    [DeviceId]              NVARCHAR (50) NULL,
    [IMEI]                  NVARCHAR (50) NULL,
    [CommandType]           NVARCHAR (50) NULL,
    [StatusCode]            NVARCHAR (50) NULL,
    [Latitude]              NVARCHAR (50) NULL,
    [Longitude]             NVARCHAR (50) NULL,
    [Speed]                 NVARCHAR (50) NULL,
    [Direction]             NVARCHAR (50) NULL,
    [Altitude]              NVARCHAR (50) NULL,
    [Mileage]               NVARCHAR (50) NULL,
    [Odometer]              INT           NULL,
    [OnBattery]             INT           NULL,
    [OnIgnition]            INT           NULL,
    [OnAc]                  INT           NULL,
    [OnGps]                 INT           NULL,
    [OnAcc]                 INT           NULL,
    [OilNElectricConected]  INT           NULL,
    [OnSOS]                 INT           NULL,
    [OnLowBattery]          INT           NULL,
    [OnPowerCut]            INT           NULL,
    [OnShock]               INT           NULL,
    [OnCharge]              INT           NULL,
    [OnDefence]             INT           NULL,
    [VoltageLevel]          INT           NULL,
    [SignalStrengthLevel]   INT           NULL,
    [AlarmType]             INT           NULL,
    [TrackerIp]             NVARCHAR (50) NULL,
    [DeviceDataTime]        DATETIME      NULL,
    [TrackerConnectedTime]  DATETIME      NULL,
    [TrackerDataActionTime] DATETIME      NULL,
    [TrackerDataParsedTime] DATETIME      NULL,
    [ActionTime]            DATETIME      NULL,
    [DeviceAlertId]         INT           NOT NULL,
    [IsSent]                BIT           DEFAULT ((0)) NULL,
    [SentTime]              DATETIME      NULL,
    [ConditionState]        BIT           NULL,
    [ConditionStateTime]    DATETIME      NULL);


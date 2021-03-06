namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class last01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReceiveDataLasts", "DataType", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Rtime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "Ptime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "IsPos", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Pos", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "PostMode", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Lat", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Lng", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Gspeed", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Ghigh", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GsDirection", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "SeeSatelliteCount", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "UseSatelliteCount", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "MCC", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "MNC", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "LAC", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "CID", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "East", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "North", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsType", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "WorkStatue", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "TootalWorkTime", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "WorkModel", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "Timing", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "SleepTime", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "WorkTime", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "BatteryVoltage", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "BlindDataCount", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "BlindSign", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "StartCount", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "IntervalTime", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "HardwareVersion", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "SoftVersion", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "ProtocolVersion", c => c.String());
            AddColumn("dbo.ReceiveDatas", "DataType", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Rtime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDatas", "Ptime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDatas", "IsPos", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Pos", c => c.String());
            AddColumn("dbo.ReceiveDatas", "PostMode", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Lat", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Lng", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Gspeed", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Ghigh", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GsDirection", c => c.String());
            AddColumn("dbo.ReceiveDatas", "SeeSatelliteCount", c => c.String());
            AddColumn("dbo.ReceiveDatas", "UseSatelliteCount", c => c.String());
            AddColumn("dbo.ReceiveDatas", "MCC", c => c.String());
            AddColumn("dbo.ReceiveDatas", "MNC", c => c.String());
            AddColumn("dbo.ReceiveDatas", "LAC", c => c.String());
            AddColumn("dbo.ReceiveDatas", "CID", c => c.String());
            AddColumn("dbo.ReceiveDatas", "East", c => c.String());
            AddColumn("dbo.ReceiveDatas", "North", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsType", c => c.String());
            AddColumn("dbo.ReceiveDatas", "WorkStatue", c => c.String());
            AddColumn("dbo.ReceiveDatas", "TootalWorkTime", c => c.String());
            AddColumn("dbo.ReceiveDatas", "WorkModel", c => c.String());
            AddColumn("dbo.ReceiveDatas", "Timing", c => c.String());
            AddColumn("dbo.ReceiveDatas", "SleepTime", c => c.String());
            AddColumn("dbo.ReceiveDatas", "WorkTime", c => c.String());
            AddColumn("dbo.ReceiveDatas", "BatteryVoltage", c => c.String());
            AddColumn("dbo.ReceiveDatas", "BlindDataCount", c => c.String());
            AddColumn("dbo.ReceiveDatas", "BlindSign", c => c.String());
            AddColumn("dbo.ReceiveDatas", "StartCount", c => c.String());
            AddColumn("dbo.ReceiveDatas", "IntervalTime", c => c.String());
            AddColumn("dbo.ReceiveDatas", "HardwareVersion", c => c.String());
            AddColumn("dbo.ReceiveDatas", "SoftVersion", c => c.String());
            AddColumn("dbo.ReceiveDatas", "ProtocolVersion", c => c.String());
            DropColumn("dbo.ReceiveDataLasts", "ReceiveTime");
            DropColumn("dbo.ReceiveDataLasts", "CollectTime");
            DropColumn("dbo.ReceiveDataLasts", "TotalWorkTime");
            DropColumn("dbo.ReceiveDataLasts", "TotalMileage");
            DropColumn("dbo.ReceiveDataLasts", "GpsTime");
            DropColumn("dbo.ReceiveDataLasts", "GpsIsPos");
            DropColumn("dbo.ReceiveDataLasts", "GpsPosProvince");
            DropColumn("dbo.ReceiveDataLasts", "GpsPos");
            DropColumn("dbo.ReceiveDataLasts", "GpsPlat");
            DropColumn("dbo.ReceiveDataLasts", "GpsPlog");
            DropColumn("dbo.ReceiveDataLasts", "GpsSpeed");
            DropColumn("dbo.ReceiveDataLasts", "GpsDirection");
            DropColumn("dbo.ReceiveDataLasts", "GpsElevation");
            DropColumn("dbo.ReceiveDataLasts", "CellInformationMain");
            DropColumn("dbo.ReceiveDataLasts", "CellInformation1");
            DropColumn("dbo.ReceiveDataLasts", "CellInformation2");
            DropColumn("dbo.ReceiveDataLasts", "CellInformation3");
            DropColumn("dbo.ReceiveDataLasts", "OutStatus");
            DropColumn("dbo.ReceiveDataLasts", "SleepStatus");
            DropColumn("dbo.ReceiveDataLasts", "AlarmStatus");
            DropColumn("dbo.ReceiveDataLasts", "LockStatus");
            DropColumn("dbo.ReceiveDataLasts", "F1");
            DropColumn("dbo.ReceiveDataLasts", "F2");
            DropColumn("dbo.ReceiveDataLasts", "F3");
            DropColumn("dbo.ReceiveDataLasts", "F4");
            DropColumn("dbo.ReceiveDataLasts", "F5");
            DropColumn("dbo.ReceiveDataLasts", "F6");
            DropColumn("dbo.ReceiveDataLasts", "F7");
            DropColumn("dbo.ReceiveDataLasts", "F8");
            DropColumn("dbo.ReceiveDataLasts", "F9");
            DropColumn("dbo.ReceiveDataLasts", "F10");
            DropColumn("dbo.ReceiveDataLasts", "F11");
            DropColumn("dbo.ReceiveDataLasts", "F12");
            DropColumn("dbo.ReceiveDataLasts", "F13");
            DropColumn("dbo.ReceiveDataLasts", "F14");
            DropColumn("dbo.ReceiveDataLasts", "F15");
            DropColumn("dbo.ReceiveDataLasts", "F16");
            DropColumn("dbo.ReceiveDataLasts", "F17");
            DropColumn("dbo.ReceiveDataLasts", "F18");
            DropColumn("dbo.ReceiveDataLasts", "F19");
            DropColumn("dbo.ReceiveDataLasts", "F20");
            DropColumn("dbo.ReceiveDataLasts", "F21");
            DropColumn("dbo.ReceiveDataLasts", "F22");
            DropColumn("dbo.ReceiveDataLasts", "F23");
            DropColumn("dbo.ReceiveDataLasts", "F24");
            DropColumn("dbo.ReceiveDataLasts", "F25");
            DropColumn("dbo.ReceiveDataLasts", "F26");
            DropColumn("dbo.ReceiveDataLasts", "F27");
            DropColumn("dbo.ReceiveDataLasts", "F28");
            DropColumn("dbo.ReceiveDataLasts", "F29");
            DropColumn("dbo.ReceiveDataLasts", "F30");
            DropColumn("dbo.ReceiveDataLasts", "F31");
            DropColumn("dbo.ReceiveDataLasts", "F32");
            DropColumn("dbo.ReceiveDataLasts", "F33");
            DropColumn("dbo.ReceiveDataLasts", "F34");
            DropColumn("dbo.ReceiveDataLasts", "F35");
            DropColumn("dbo.ReceiveDataLasts", "F36");
            DropColumn("dbo.ReceiveDataLasts", "F37");
            DropColumn("dbo.ReceiveDataLasts", "F38");
            DropColumn("dbo.ReceiveDataLasts", "F39");
            DropColumn("dbo.ReceiveDataLasts", "F40");
            DropColumn("dbo.ReceiveDatas", "ReceiveTime");
            DropColumn("dbo.ReceiveDatas", "CollectTime");
            DropColumn("dbo.ReceiveDatas", "TotalWorkTime");
            DropColumn("dbo.ReceiveDatas", "TotalMileage");
            DropColumn("dbo.ReceiveDatas", "GpsTime");
            DropColumn("dbo.ReceiveDatas", "GpsIsPos");
            DropColumn("dbo.ReceiveDatas", "GpsPosProvince");
            DropColumn("dbo.ReceiveDatas", "GpsPos");
            DropColumn("dbo.ReceiveDatas", "GpsPlat");
            DropColumn("dbo.ReceiveDatas", "GpsPlog");
            DropColumn("dbo.ReceiveDatas", "GpsSpeed");
            DropColumn("dbo.ReceiveDatas", "GpsDirection");
            DropColumn("dbo.ReceiveDatas", "GpsElevation");
            DropColumn("dbo.ReceiveDatas", "CellInformationMain");
            DropColumn("dbo.ReceiveDatas", "CellInformation1");
            DropColumn("dbo.ReceiveDatas", "CellInformation2");
            DropColumn("dbo.ReceiveDatas", "CellInformation3");
            DropColumn("dbo.ReceiveDatas", "OutStatus");
            DropColumn("dbo.ReceiveDatas", "SleepStatus");
            DropColumn("dbo.ReceiveDatas", "AlarmStatus");
            DropColumn("dbo.ReceiveDatas", "LockStatus");
            DropColumn("dbo.ReceiveDatas", "F1");
            DropColumn("dbo.ReceiveDatas", "F2");
            DropColumn("dbo.ReceiveDatas", "F3");
            DropColumn("dbo.ReceiveDatas", "F4");
            DropColumn("dbo.ReceiveDatas", "F5");
            DropColumn("dbo.ReceiveDatas", "F6");
            DropColumn("dbo.ReceiveDatas", "F7");
            DropColumn("dbo.ReceiveDatas", "F8");
            DropColumn("dbo.ReceiveDatas", "F9");
            DropColumn("dbo.ReceiveDatas", "F10");
            DropColumn("dbo.ReceiveDatas", "F11");
            DropColumn("dbo.ReceiveDatas", "F12");
            DropColumn("dbo.ReceiveDatas", "F13");
            DropColumn("dbo.ReceiveDatas", "F14");
            DropColumn("dbo.ReceiveDatas", "F15");
            DropColumn("dbo.ReceiveDatas", "F16");
            DropColumn("dbo.ReceiveDatas", "F17");
            DropColumn("dbo.ReceiveDatas", "F18");
            DropColumn("dbo.ReceiveDatas", "F19");
            DropColumn("dbo.ReceiveDatas", "F20");
            DropColumn("dbo.ReceiveDatas", "F21");
            DropColumn("dbo.ReceiveDatas", "F22");
            DropColumn("dbo.ReceiveDatas", "F23");
            DropColumn("dbo.ReceiveDatas", "F24");
            DropColumn("dbo.ReceiveDatas", "F25");
            DropColumn("dbo.ReceiveDatas", "F26");
            DropColumn("dbo.ReceiveDatas", "F27");
            DropColumn("dbo.ReceiveDatas", "F28");
            DropColumn("dbo.ReceiveDatas", "F29");
            DropColumn("dbo.ReceiveDatas", "F30");
            DropColumn("dbo.ReceiveDatas", "F31");
            DropColumn("dbo.ReceiveDatas", "F32");
            DropColumn("dbo.ReceiveDatas", "F33");
            DropColumn("dbo.ReceiveDatas", "F34");
            DropColumn("dbo.ReceiveDatas", "F35");
            DropColumn("dbo.ReceiveDatas", "F36");
            DropColumn("dbo.ReceiveDatas", "F37");
            DropColumn("dbo.ReceiveDatas", "F38");
            DropColumn("dbo.ReceiveDatas", "F39");
            DropColumn("dbo.ReceiveDatas", "F40");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReceiveDatas", "F40", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F39", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F38", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F37", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F36", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F35", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F34", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F33", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F32", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F31", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F30", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F29", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F28", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F27", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F26", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F25", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F24", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F23", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F22", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F21", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F20", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F19", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F18", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F17", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F16", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F15", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F14", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F13", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F12", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F11", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F10", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F9", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F8", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F7", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F6", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F5", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F4", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F3", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F2", c => c.String());
            AddColumn("dbo.ReceiveDatas", "F1", c => c.String());
            AddColumn("dbo.ReceiveDatas", "LockStatus", c => c.String());
            AddColumn("dbo.ReceiveDatas", "AlarmStatus", c => c.String());
            AddColumn("dbo.ReceiveDatas", "SleepStatus", c => c.String());
            AddColumn("dbo.ReceiveDatas", "OutStatus", c => c.String());
            AddColumn("dbo.ReceiveDatas", "CellInformation3", c => c.String());
            AddColumn("dbo.ReceiveDatas", "CellInformation2", c => c.String());
            AddColumn("dbo.ReceiveDatas", "CellInformation1", c => c.String());
            AddColumn("dbo.ReceiveDatas", "CellInformationMain", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsElevation", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsDirection", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsSpeed", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsPlog", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsPlat", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsPos", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsPosProvince", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsIsPos", c => c.String());
            AddColumn("dbo.ReceiveDatas", "GpsTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDatas", "TotalMileage", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDatas", "TotalWorkTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDatas", "CollectTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDatas", "ReceiveTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "F40", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F39", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F38", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F37", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F36", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F35", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F34", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F33", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F32", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F31", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F30", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F29", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F28", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F27", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F26", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F25", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F24", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F23", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F22", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F21", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F20", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F19", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F18", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F17", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F16", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F15", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F14", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F13", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F12", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F11", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F10", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F9", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F8", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F7", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F6", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F5", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F4", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F3", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F2", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "F1", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "LockStatus", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "AlarmStatus", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "SleepStatus", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "OutStatus", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "CellInformation3", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "CellInformation2", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "CellInformation1", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "CellInformationMain", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsElevation", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsDirection", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsSpeed", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsPlog", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsPlat", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsPos", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsPosProvince", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsIsPos", c => c.String());
            AddColumn("dbo.ReceiveDataLasts", "GpsTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "TotalMileage", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "TotalWorkTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "CollectTime", c => c.Long(nullable: false));
            AddColumn("dbo.ReceiveDataLasts", "ReceiveTime", c => c.Long(nullable: false));
            DropColumn("dbo.ReceiveDatas", "ProtocolVersion");
            DropColumn("dbo.ReceiveDatas", "SoftVersion");
            DropColumn("dbo.ReceiveDatas", "HardwareVersion");
            DropColumn("dbo.ReceiveDatas", "IntervalTime");
            DropColumn("dbo.ReceiveDatas", "StartCount");
            DropColumn("dbo.ReceiveDatas", "BlindSign");
            DropColumn("dbo.ReceiveDatas", "BlindDataCount");
            DropColumn("dbo.ReceiveDatas", "BatteryVoltage");
            DropColumn("dbo.ReceiveDatas", "WorkTime");
            DropColumn("dbo.ReceiveDatas", "SleepTime");
            DropColumn("dbo.ReceiveDatas", "Timing");
            DropColumn("dbo.ReceiveDatas", "WorkModel");
            DropColumn("dbo.ReceiveDatas", "TootalWorkTime");
            DropColumn("dbo.ReceiveDatas", "WorkStatue");
            DropColumn("dbo.ReceiveDatas", "GpsType");
            DropColumn("dbo.ReceiveDatas", "North");
            DropColumn("dbo.ReceiveDatas", "East");
            DropColumn("dbo.ReceiveDatas", "CID");
            DropColumn("dbo.ReceiveDatas", "LAC");
            DropColumn("dbo.ReceiveDatas", "MNC");
            DropColumn("dbo.ReceiveDatas", "MCC");
            DropColumn("dbo.ReceiveDatas", "UseSatelliteCount");
            DropColumn("dbo.ReceiveDatas", "SeeSatelliteCount");
            DropColumn("dbo.ReceiveDatas", "GsDirection");
            DropColumn("dbo.ReceiveDatas", "Ghigh");
            DropColumn("dbo.ReceiveDatas", "Gspeed");
            DropColumn("dbo.ReceiveDatas", "Lng");
            DropColumn("dbo.ReceiveDatas", "Lat");
            DropColumn("dbo.ReceiveDatas", "PostMode");
            DropColumn("dbo.ReceiveDatas", "Pos");
            DropColumn("dbo.ReceiveDatas", "IsPos");
            DropColumn("dbo.ReceiveDatas", "Ptime");
            DropColumn("dbo.ReceiveDatas", "Rtime");
            DropColumn("dbo.ReceiveDatas", "DataType");
            DropColumn("dbo.ReceiveDataLasts", "ProtocolVersion");
            DropColumn("dbo.ReceiveDataLasts", "SoftVersion");
            DropColumn("dbo.ReceiveDataLasts", "HardwareVersion");
            DropColumn("dbo.ReceiveDataLasts", "IntervalTime");
            DropColumn("dbo.ReceiveDataLasts", "StartCount");
            DropColumn("dbo.ReceiveDataLasts", "BlindSign");
            DropColumn("dbo.ReceiveDataLasts", "BlindDataCount");
            DropColumn("dbo.ReceiveDataLasts", "BatteryVoltage");
            DropColumn("dbo.ReceiveDataLasts", "WorkTime");
            DropColumn("dbo.ReceiveDataLasts", "SleepTime");
            DropColumn("dbo.ReceiveDataLasts", "Timing");
            DropColumn("dbo.ReceiveDataLasts", "WorkModel");
            DropColumn("dbo.ReceiveDataLasts", "TootalWorkTime");
            DropColumn("dbo.ReceiveDataLasts", "WorkStatue");
            DropColumn("dbo.ReceiveDataLasts", "GpsType");
            DropColumn("dbo.ReceiveDataLasts", "North");
            DropColumn("dbo.ReceiveDataLasts", "East");
            DropColumn("dbo.ReceiveDataLasts", "CID");
            DropColumn("dbo.ReceiveDataLasts", "LAC");
            DropColumn("dbo.ReceiveDataLasts", "MNC");
            DropColumn("dbo.ReceiveDataLasts", "MCC");
            DropColumn("dbo.ReceiveDataLasts", "UseSatelliteCount");
            DropColumn("dbo.ReceiveDataLasts", "SeeSatelliteCount");
            DropColumn("dbo.ReceiveDataLasts", "GsDirection");
            DropColumn("dbo.ReceiveDataLasts", "Ghigh");
            DropColumn("dbo.ReceiveDataLasts", "Gspeed");
            DropColumn("dbo.ReceiveDataLasts", "Lng");
            DropColumn("dbo.ReceiveDataLasts", "Lat");
            DropColumn("dbo.ReceiveDataLasts", "PostMode");
            DropColumn("dbo.ReceiveDataLasts", "Pos");
            DropColumn("dbo.ReceiveDataLasts", "IsPos");
            DropColumn("dbo.ReceiveDataLasts", "Ptime");
            DropColumn("dbo.ReceiveDataLasts", "Rtime");
            DropColumn("dbo.ReceiveDataLasts", "DataType");
        }
    }
}

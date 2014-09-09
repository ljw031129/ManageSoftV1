namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatePm : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PmDataBits",
                c => new
                    {
                        PmDataBitId = c.String(nullable: false, maxLength: 128),
                        Representation = c.Int(nullable: false),
                        ByteCounts = c.Int(nullable: false),
                        IsBigEndian = c.Boolean(nullable: false),
                        BitType = c.Int(nullable: false),
                        ChildStartPostion = c.Int(nullable: false),
                        ChildDataLength = c.Int(nullable: false),
                        DefaultValue = c.String(),
                        DictionaryKey = c.String(),
                        PmDataBody_PmDataBodyId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PmDataBitId)
                .ForeignKey("dbo.PmDataBodies", t => t.PmDataBody_PmDataBodyId)
                .Index(t => t.PmDataBody_PmDataBodyId);
            
            CreateTable(
                "dbo.PmDataBodies",
                c => new
                    {
                        PmDataBodyId = c.String(nullable: false, maxLength: 128),
                        InfoTypeNumber = c.String(),
                        StartPosition = c.Int(nullable: false),
                        DataLength = c.Int(nullable: false),
                        DataType = c.Int(nullable: false),
                        PmDataByte_PmDataByteId = c.String(maxLength: 128),
                        PmFInterpreter_PmFInterpreterId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PmDataBodyId)
                .ForeignKey("dbo.PmDataBytes", t => t.PmDataByte_PmDataByteId)
                .ForeignKey("dbo.PmFInterpreters", t => t.PmFInterpreter_PmFInterpreterId)
                .Index(t => t.PmDataByte_PmDataByteId)
                .Index(t => t.PmFInterpreter_PmFInterpreterId);
            
            CreateTable(
                "dbo.PmDataBytes",
                c => new
                    {
                        PmDataByteId = c.String(nullable: false, maxLength: 128),
                        Representation = c.Int(nullable: false),
                        IsBigEndian = c.Boolean(nullable: false),
                        IsUnsigned = c.Boolean(nullable: false),
                        Formula = c.String(),
                        DefaultValue = c.String(),
                        DictionaryKey = c.String(),
                    })
                .PrimaryKey(t => t.PmDataByteId);
            
            CreateTable(
                "dbo.PmFInterpreters",
                c => new
                    {
                        PmFInterpreterId = c.String(nullable: false, maxLength: 128),
                        ProtocolName = c.String(),
                        ProtocolVersion = c.String(),
                        AnalysisWay = c.Int(nullable: false),
                        InfoCountsPostion = c.Int(nullable: false),
                        InfoSignStartPosition = c.Int(nullable: false),
                        InfoSignLength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PmFInterpreterId);
            
            CreateTable(
                "dbo.PmSpeciaCalculations",
                c => new
                    {
                        PmSpeciaCalculationId = c.String(nullable: false, maxLength: 128),
                        SrcDictionaryKey = c.String(),
                        Formula = c.String(),
                        TargetDictionaryKey = c.String(),
                        PmFInterpreter_PmFInterpreterId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.PmSpeciaCalculationId)
                .ForeignKey("dbo.PmFInterpreters", t => t.PmFInterpreter_PmFInterpreterId)
                .Index(t => t.PmFInterpreter_PmFInterpreterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PmSpeciaCalculations", "PmFInterpreter_PmFInterpreterId", "dbo.PmFInterpreters");
            DropForeignKey("dbo.PmDataBodies", "PmFInterpreter_PmFInterpreterId", "dbo.PmFInterpreters");
            DropForeignKey("dbo.PmDataBodies", "PmDataByte_PmDataByteId", "dbo.PmDataBytes");
            DropForeignKey("dbo.PmDataBits", "PmDataBody_PmDataBodyId", "dbo.PmDataBodies");
            DropIndex("dbo.PmSpeciaCalculations", new[] { "PmFInterpreter_PmFInterpreterId" });
            DropIndex("dbo.PmDataBodies", new[] { "PmFInterpreter_PmFInterpreterId" });
            DropIndex("dbo.PmDataBodies", new[] { "PmDataByte_PmDataByteId" });
            DropIndex("dbo.PmDataBits", new[] { "PmDataBody_PmDataBodyId" });
            DropTable("dbo.PmSpeciaCalculations");
            DropTable("dbo.PmFInterpreters");
            DropTable("dbo.PmDataBytes");
            DropTable("dbo.PmDataBodies");
            DropTable("dbo.PmDataBits");
        }
    }
}

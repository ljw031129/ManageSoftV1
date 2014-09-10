namespace SocialGoal.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PmDataBits", name: "PmDataBody_PmDataBodyId", newName: "PmDataBodyId");
            RenameColumn(table: "dbo.PmDataBodies", name: "PmDataByte_PmDataByteId", newName: "PmDataByteId");
            RenameColumn(table: "dbo.PmDataBodies", name: "PmFInterpreter_PmFInterpreterId", newName: "PmFInterpreterId");
            RenameIndex(table: "dbo.PmDataBits", name: "IX_PmDataBody_PmDataBodyId", newName: "IX_PmDataBodyId");
            RenameIndex(table: "dbo.PmDataBodies", name: "IX_PmFInterpreter_PmFInterpreterId", newName: "IX_PmFInterpreterId");
            RenameIndex(table: "dbo.PmDataBodies", name: "IX_PmDataByte_PmDataByteId", newName: "IX_PmDataByteId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PmDataBodies", name: "IX_PmDataByteId", newName: "IX_PmDataByte_PmDataByteId");
            RenameIndex(table: "dbo.PmDataBodies", name: "IX_PmFInterpreterId", newName: "IX_PmFInterpreter_PmFInterpreterId");
            RenameIndex(table: "dbo.PmDataBits", name: "IX_PmDataBodyId", newName: "IX_PmDataBody_PmDataBodyId");
            RenameColumn(table: "dbo.PmDataBodies", name: "PmFInterpreterId", newName: "PmFInterpreter_PmFInterpreterId");
            RenameColumn(table: "dbo.PmDataBodies", name: "PmDataByteId", newName: "PmDataByte_PmDataByteId");
            RenameColumn(table: "dbo.PmDataBits", name: "PmDataBodyId", newName: "PmDataBody_PmDataBodyId");
        }
    }
}

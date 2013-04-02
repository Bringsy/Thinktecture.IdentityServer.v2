namespace Thinktecture.IdentityServer.Core.Repositories.Migrations.SqlCe
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HrdExtensions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.IdentityProvider", "Realm", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.IdentityProvider", "Realm");
        }
    }
}

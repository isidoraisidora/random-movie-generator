namespace RandomMovieGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelSoDodaeni : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "ReleaseYear", c => c.Int(nullable: false));
            AddColumn("dbo.Movies", "Trailer", c => c.String());
            AlterColumn("dbo.Movies", "Rating", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Rating", c => c.Int(nullable: false));
            DropColumn("dbo.Movies", "Trailer");
            DropColumn("dbo.Movies", "ReleaseYear");
        }
    }
}

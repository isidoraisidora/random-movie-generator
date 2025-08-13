namespace RandomMovieGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class soYourRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "YourRating", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "YourRating");
        }
    }
}

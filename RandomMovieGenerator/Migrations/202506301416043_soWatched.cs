namespace RandomMovieGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class soWatched : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Watcheds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        MovieId = c.Int(nullable: false),
                        Rating = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Watcheds", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Watcheds", "MovieId", "dbo.Movies");
            DropIndex("dbo.Watcheds", new[] { "MovieId" });
            DropIndex("dbo.Watcheds", new[] { "UserId" });
            DropTable("dbo.Watcheds");
        }
    }
}

namespace RandomMovieGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SoWatchlist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Watchlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        MovieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.MovieId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.MovieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Watchlists", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Watchlists", "MovieId", "dbo.Movies");
            DropIndex("dbo.Watchlists", new[] { "MovieId" });
            DropIndex("dbo.Watchlists", new[] { "UserId" });
            DropTable("dbo.Watchlists");
        }
    }
}

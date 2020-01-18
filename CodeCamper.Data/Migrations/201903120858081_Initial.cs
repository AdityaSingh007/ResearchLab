namespace CodeCamper.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendance",
                c => new
                    {
                        SessionId = c.Int(nullable: false),
                        PersonId = c.Int(nullable: false),
                        Rating = c.Int(nullable: false),
                        Text = c.String(),
                    })
                .PrimaryKey(t => new { t.SessionId, t.PersonId })
                .ForeignKey("dbo.Person", t => t.PersonId)
                .ForeignKey("dbo.Session", t => t.SessionId)
                .Index(t => t.SessionId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Blog = c.String(),
                        Twitter = c.String(),
                        Gender = c.String(maxLength: 1),
                        ImageSource = c.String(),
                        Bio = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Session",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Title = c.String(nullable: false),
                        Code = c.String(),
                        SpeakerId = c.Int(nullable: false),
                        TrackId = c.Int(nullable: false),
                        TimeSlotId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        Level = c.String(),
                        Tags = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Room", t => t.RoomId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.SpeakerId, cascadeDelete: true)
                .ForeignKey("dbo.TimeSlot", t => t.TimeSlotId, cascadeDelete: true)
                .ForeignKey("dbo.Track", t => t.TrackId, cascadeDelete: true)
                .Index(t => t.SpeakerId)
                .Index(t => t.TrackId)
                .Index(t => t.TimeSlotId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeSlot",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Start = c.DateTime(nullable: false),
                        IsSessionSlot = c.Boolean(nullable: false),
                        Duration = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Track",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Attendance", "SessionId", "dbo.Session");
            DropForeignKey("dbo.Attendance", "PersonId", "dbo.Person");
            DropForeignKey("dbo.Session", "TrackId", "dbo.Track");
            DropForeignKey("dbo.Session", "TimeSlotId", "dbo.TimeSlot");
            DropForeignKey("dbo.Session", "SpeakerId", "dbo.Person");
            DropForeignKey("dbo.Session", "RoomId", "dbo.Room");
            DropIndex("dbo.Session", new[] { "RoomId" });
            DropIndex("dbo.Session", new[] { "TimeSlotId" });
            DropIndex("dbo.Session", new[] { "TrackId" });
            DropIndex("dbo.Session", new[] { "SpeakerId" });
            DropIndex("dbo.Attendance", new[] { "PersonId" });
            DropIndex("dbo.Attendance", new[] { "SessionId" });
            DropTable("dbo.Track");
            DropTable("dbo.TimeSlot");
            DropTable("dbo.Room");
            DropTable("dbo.Session");
            DropTable("dbo.Person");
            DropTable("dbo.Attendance");
        }
    }
}

using System;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebApplication14;
using System.Net.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc;
using WebApplication14.Models;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using System.Net;

namespace Integration.test
{
    public class UnitTest1
    {
        public HttpClient _client;

        Note note = new Note()
        {
            Title = "this is test title",
            PlainText = "some text",
            Pinned = true,
            labels = new List<Labels>
                {
                    new Labels{ label="My First Tag" },
                    new Labels{ label = "My second Tag" },
                    new Labels{ label = "My third Tag" },
                },
            checklists = new List<CheckList>
                {
                new CheckList{checkList="first item" , isChecked = true},
                new CheckList{checkList="second item", isChecked = true},
                new CheckList{checkList="third item", isChecked = true},
                }
        };

        Note note2 = new Note()
        {
            Title = "this is deleted title",
            PlainText = "some text",
            Pinned = false,
            labels = new List<Labels>()
                {
                    new Labels(){ label="My First Tag" },
                    new Labels(){ label = "My second Tag" },
                    new Labels(){ label = "My third Tag" },
                },
            checklists = new List<CheckList>()
                {
                new CheckList(){checkList="first item" , isChecked = true},
                new CheckList(){checkList="second item", isChecked = true},
                new CheckList(){checkList="third item", isChecked = true},
                }
        };

        List<Note> dbnote = new List<Note>();


        Note postnote = new Note()
        {
            Title = "this is Post title",
            PlainText = "some text Posted",
            Pinned = false,
            labels = new List<Labels>()
                {
                    new Labels(){ label="My First Tag" },
                    new Labels(){ label = "My second Tag" },
                    new Labels(){ label = "My third Tag" },
                },
            checklists = new List<CheckList>()
                {
                new CheckList(){checkList="first item" , isChecked = true},
                new CheckList(){checkList="second item", isChecked = true},
                new CheckList(){checkList="third item", isChecked = true},
                }
        };

        Note putnote2 = new Note()
        {
            Id = 2,
            Title = "this is Changed title",
            PlainText = "some text",
            Pinned = false,
            labels = new List<Labels>
                {
                    new Labels{ Id=4, label="My First Tag" },
                    new Labels{ Id=5, label = "My second Tag" },
                    new Labels{ Id=6, label = "My third Tag" },
                },
            checklists = new List<CheckList>
                {
                new CheckList{ Id=4, checkList="first item" , isChecked = true},
                new CheckList{ Id=5, checkList="second item", isChecked = true},
                new CheckList{ Id=6, checkList="third item", isChecked = true},
                }
        };

        public  NoteContext _context;

        public UnitTest1()
        {
            var builder = new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>();

            TestServer testServer = new TestServer(builder);
            
            _client = testServer.CreateClient();
            _context = testServer.Host.Services.GetRequiredService<NoteContext>();
            dbnote.Add(note);
            dbnote.Add(note2);
            _context.Note.Add(note);
            _context.Note.Add(note2);
            _context.SaveChanges();
        }

        [Fact]
        public async void GetData()
        {
            var response =await _client.GetAsync("/api/Notes");

            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            

            Assert.NotNull(result);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();

            var notes = JsonConvert.DeserializeObject<List<Note>>(responseString);

            responseString.Should().Contain("this is test title")
              .And.Contain("some text")
              .And.Contain("true");

            Assert.True(notes.TrueForAll(x => dbnote.Exists(y => y.IsEquals(x))));

        }


        [Fact]
        public async void PostData()
        {
            HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "api/Notes")
            {
                Content = new StringContent(JsonConvert.SerializeObject(postnote), UnicodeEncoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(postMessage);
            
            var responseString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Note>(responseString);
            
            response.EnsureSuccessStatusCode();

            Assert.True(postnote.IsEquals(obj));

        }

        [Fact]
        public async void PutNote()
        {
            HttpRequestMessage putMessage = new HttpRequestMessage(HttpMethod.Put, "api/Notes/2")
            {
                Content = new StringContent(JsonConvert.SerializeObject(putnote2), Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(putMessage);

            var responseString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Note>(responseString);

            var statuscode = response.StatusCode;
            Assert.Equal(HttpStatusCode.Created, statuscode);

            Assert.True(putnote2.IsEquals(obj));
        }

        [Fact]
        public async void deleteNote()
        {
            var response = await _client.DeleteAsync("/api/Notes/?Title=this is test title");
            var statuscode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, statuscode);
            _context.Note.Should().NotContain("this is test title");
        }

    }
}

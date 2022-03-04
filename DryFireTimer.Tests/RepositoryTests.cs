using DryFireTimer.DataAccess;
using DryFireTimer.Models;
using System;
using System.IO;
using Xunit;

namespace DryFireTimer.Tests
{
    public class RepositoryTests : IDisposable
    {
        private static readonly string dbpath = @"C:\MyFiles\Coding\DryFireTimer\DryFireTimer.Tests\testDB\test.sqlite";
        private readonly IMyRepository repo = new Repository(dbpath);

        private void Arrange()
        {
            for (int i = 2; i < 11; i++)
                repo.Create(new Exercise() {Id = i});
        }

        [Fact]
        public void GetsFirstItem()
        {
            //Arrange
            Arrange();
            //Act
            int x = repo.GetFirst().Id;
            //Assert
            Assert.Equal(1, x);
        }
        [Fact]
        public void GetsNextItem()
        {
            //Arrange
            Arrange();
            //Act
            int x = repo.GetNext(new Exercise() {Id = 5 }).Id;
            //Assert
            Assert.Equal(6, x);
        }
        [Fact]
        public void GetsPreviousItem()
        {
            //Arrange
            Arrange();
            //Act
            int x = repo.GetPrevious(new Exercise() { Id = 5 }).Id;
            //Assert
            Assert.Equal(4, x);
        }
        [Fact]
        public void GetsNullAtTheEdges()
        {
            //Arrange
            Arrange();
            //Act
            var x = repo.GetPrevious(new Exercise() { Id = 1 });
            var y = repo.GetNext(new Exercise() { Id = 10 });
            //Assert
            Assert.Null(x);
            Assert.Null(y);
        }

        public void Dispose()
        {
            File.Delete(dbpath);
        }
    }
}

using JetBrains.Annotations;
using Xunit;
using System;


namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{
    private readonly UserService _userService;
    private readonly UserTest _userTest;
    
    public UserServiceTest()
    {
        _userService = new UserService();
        _userTest = new UserTest
        {
            FirstName = "Joe", LastName = "Doe", EmailAdress = "johndoe@gmail.com",
            DateOfBirth = DateTime.Parse("1982-03-21"), ClientId = 1
        };
    }

    [Fact]
    public void AddUser_Should_Return_False_When_FirstName_Is_Empty()
    {
        _userTest.FirstName = "";
        var result = _userService.AddUser(_userTest.FirstName, _userTest.LastName, _userTest.EmailAdress,
            _userTest.DateOfBirth, _userTest.ClientId);
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_LastName_Is_Empty()
    {
        _userTest.LastName = "";
        var result = _userService.AddUser(_userTest.FirstName, _userTest.LastName, _userTest.EmailAdress,
            _userTest.DateOfBirth, _userTest.ClientId);
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Email_Is_Invalid()
    {
        _userTest.EmailAdress = "johndoegmailcom";
        var result = _userService.AddUser(_userTest.FirstName, _userTest.LastName, _userTest.EmailAdress,
            _userTest.DateOfBirth, _userTest.ClientId);
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_Age_Is_Under_21()
    {
        _userTest.DateOfBirth = DateTime.Now.AddYears(-20);
        var result = _userService.AddUser(_userTest.FirstName, _userTest.LastName, _userTest.EmailAdress,
            _userTest.DateOfBirth, _userTest.ClientId);
        Assert.False(result);
    }

    [Fact]
    public void AddUser_Should_Return_True_When_All_Conditions_Are_Met()
    {
        var result = _userService.AddUser(_userTest.FirstName, _userTest.LastName, _userTest.EmailAdress,
            _userTest.DateOfBirth, _userTest.ClientId);
        Assert.True(result);
    }

    [Fact]
    public void CalculateAge_Should_Return_20_The_Day_Before_21st_Birthday()
    {
        var dateOfBirth = DateTime.Today.AddYears(-21).AddDays(1);
        var age = _userService.CalculateAge(dateOfBirth);
        Assert.Equal(20, age);
    }

    [Fact]
    public void CalculateAge_Should_Return_21_On_21st_Birthday()
    {
        var dateOfBirth = DateTime.Today.AddYears(-21);
        var age = _userService.CalculateAge(dateOfBirth);
        Assert.Equal(21, age);
    }

    [Fact]
    public void SetCreditLimit_Should_Not_Set_CreditLimit_For_VeryImportantClient()
    {
        var user = new User();
        var client = new Client { Type = "VeryImportantClient" };
        bool result = _userService.SetCreditLimit(user, client);
        Assert.False(user.HasCreditLimit);
        Assert.True(result);
    }
}


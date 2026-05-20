using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    [TestMethod]
    public void ReserveSeat_Works()
    {

        // Arrange
        
        Mock<SeatsService> mockService = new Mock<SeatsService>(); //Création d'un faux service.
        

        //Ici on crée le faux siège.
        Seat seat = new Seat()
        {
            Number = 10
        };


        //Le .Setup dans le ReserveSeat(), ya deux parametre donc dans le .Setup cest pour sa qu'on fait It.IsAny<string>(), car dans 
        //researveSeat, le premier parametre est le id du user donc la on dit que peux importe le id du user, on retoure le siege 10
        mockService
            .Setup(s => s.ReserveSeat(It.IsAny<string>(), 10))
            .Returns(seat);


        //Creation du controller, on creer un nouveauc controller, et on mets le faux service suivi de .Object
        SeatsController controller = new SeatsController(mockService.Object);


        
        // Act
        ActionResult<Seat> result = controller.ReserveSeat(10); //Ici on appel la methodeReserveSeat avec le siege 10

        // Assert
        OkObjectResult okResult = result.Result as OkObjectResult; 

        Assert.IsNotNull(okResult);

        Seat? returnedSeat = okResult.Value as Seat;

        Assert.AreEqual(10, returnedSeat.Number);
    }

    [TestMethod]
    public void ReserveSeat_Taken()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>(); //Création d'un faux service.

        Seat seat = new Seat()
        {
            Number = 10
        };

        mockService
        .Setup(s => s.ReserveSeat(It.IsAny<string>(), 10))
        .Throws(new SeatAlreadyTakenException());

        SeatsController controller = new SeatsController(mockService.Object);
            

        //ACT
        ActionResult<Seat> result = controller.ReserveSeat(10);


        UnauthorizedResult? unauthorizedResult = result.Result as UnauthorizedResult;

        Assert.IsNotNull(unauthorizedResult);
    }
    [TestMethod]
    public void ReserveSeat_Bust()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>(); //Création d'un faux service.

       

        mockService
       .Setup(s => s.ReserveSeat(It.IsAny<string>(), 101))
       .Throws(new SeatOutOfBoundsException());

        SeatsController controller = new SeatsController(mockService.Object);

        ActionResult<Seat> result = controller.ReserveSeat(101);


        NotFoundObjectResult? notFoundResult = result.Result as NotFoundObjectResult;

        Assert.IsNotNull(notFoundResult);


        Assert.AreEqual("Could not find 101 ", notFoundResult.Value);
    }

    [TestMethod]
    public void ReserveSeat_AlreadyGotOne ()
    {
        Mock<SeatsService> mockService = new Mock<SeatsService>(); //Création d'un faux service.



        mockService
       .Setup(s => s.ReserveSeat(It.IsAny<string>(), 10))
       .Throws(new UserAlreadySeatedException());

        SeatsController controller = new SeatsController(mockService.Object);

        ActionResult<Seat> result = controller.ReserveSeat(10);


        BadRequestResult? BadRequest = result.Result as BadRequestResult;

        Assert.IsNotNull(BadRequest);

    }

}

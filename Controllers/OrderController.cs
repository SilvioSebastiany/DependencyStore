using Dapper;
using DependencyStore.Models;
using DependencyStore.Repositories.Contracts;
using DependencyStore.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RestSharp;

namespace DependencyStore.Controllers;

public class OrderController : ControllerBase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeService _deliveryFeeService;
    private readonly IPromoCodeRepository _promoCodeRepository;

    public OrderController(ICustomerRepository customerRepository, IDeliveryFeeService deliveryFeeService, IPromoCodeRepository promoCodeRepository)
    {
        _customerRepository = customerRepository;
        _deliveryFeeService = deliveryFeeService;
        _promoCodeRepository = promoCodeRepository;
    }

    [Route("v1/orders")]
    [HttpPost]
    public async Task<IActionResult> Place(string customerId, string zipCode, string promoCode, int[] products)
    {
        // #1 - Recupera o cliente
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            return NotFound("Cliente não encontrado.");


        // #2 - Calcula o frete
        var deliveryFee = await _deliveryFeeService.GetDeliveryFeeAsync(zipCode);
        var cupom = await _promoCodeRepository.GetPromoCodeAsync(promoCode);
        var discount = cupom?.Value ?? 0M;

        var order = new Order(deliveryFee, discount, new List<Product>());

        // #7 - Retorna
        return Ok( $"Pedido {order.Code} gerado com sucesso!");
    }
}
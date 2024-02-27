using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }

        //getting all the stocks
        [HttpGet]
        public IActionResult GetAll(){
            var stocks = _context.Stocks.ToList()
                .Select(s=>s.ToStockDto());

            return Ok(stocks);
        }

        //getting single stock 
        [HttpGet("{id}")]
        public IActionResult GetByID([FromRoute] int id){
            var stock = _context.Stocks.Find(id);

            if(stock == null){
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto  StockDto){
            var stockModel = StockDto.ToStockFromCreateDto();
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetByID),new {id=stockModel.Id},stockModel.ToStockDto());
        }

        //update
        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto){
            //searching for el to update
            var stockModel = _context.Stocks.FirstOrDefault(x=>x.Id == id);

            if(stockModel == null){
                return NotFound();
            }
            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ChatApp.Bll.DTOs;
using ChatApp.Bll.Interfaces;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dateService;
        private readonly IMapper _mapper;
        private readonly ILogger<DataController> _logger;

        public DataController(IDataService dateService, IMapper mapper, ILogger<DataController> logger)
        {
            _dateService = dateService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Метод контролллера вызываемый из swagger
        /// </summary>
        /// <param name="cancellationToken">Отменить операцию сохрания в базу данных</param>
        /// <param name="dateTimeStart">дата с какой происходит поиск новостей (по дефолту сегодняшняя дата)</param>
        /// <param name="dateTimeEnd">дата по какую происходит поиск новостей (по дефолту сегодняшняя дата)</param>
        /// <returns></returns>
        [HttpPost("onliner")]
        public async Task<IActionResult> OnlinerNewsLoader(CancellationToken cancellationToken, DateTime? dateTimeStart = null, DateTime? dateTimeEnd = null)
        {
            var dateStart = dateTimeStart.HasValue ? dateTimeStart.Value : DateTime.Today;
            var dateEnd = dateTimeEnd.HasValue ? dateTimeEnd.Value : DateTime.Now;
            // вызов метода сервиса
            var result = await _dateService.SeedDateOnliner(_mapper.Map<JsonConfigDTO>(_config), dateStart, dateEnd, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Метод контролллера вызываемый из swagger
        /// </summary>
        /// <param name="cancellationToken">Отменить операцию сохрания в базу данных</param>
        /// <param name="dateTimeStart">дата с какой происходит поиск новостей (по дефолту сегодняшняя дата)</param>
        /// <param name="dateTimeEnd">дата по какую происходит поиск новостей (по дефолту сегодняшняя дата)</param>
        /// <returns></returns>
        [HttpPost("belta")]
        public async Task<IActionResult> BeltaNewsLoader(CancellationToken cancellationToken, DateTime? dateTimeStart = null, DateTime? dateTimeEnd = null)
        {
            var dateStart = dateTimeStart.HasValue ? dateTimeStart.Value : DateTime.Today;
            var dateEnd = dateTimeEnd.HasValue ? dateTimeEnd.Value : DateTime.Now;
            // вызов метода сервиса
            var result = await _dateService.SeedDateBelta(_mapper.Map<JsonConfigDTO>(_config), dateStart, dateEnd, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Метод контролллера вызываемый из swagger
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        [HttpGet("GetAllNews")]
        public async Task<IActionResult> GetNeswByDate(CancellationToken cancellationToken, DateTime? dateTimeStart = null, DateTime? dateTimeEnd = null)
        {
            var dateStart = dateTimeStart.HasValue ? dateTimeStart.Value : DateTime.Today;
            var dateEnd = dateTimeEnd.HasValue ? dateTimeEnd.Value : DateTime.Now;
            // вызов метода сервиса
            var result = _dateService.GetNewsByDate(dateStart, dateEnd, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Метод контролллера вызываемый из swagger
        /// </summary>
        /// <param name="links">ссылки для анализа</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("NewsLoader")]
        public async Task<IActionResult> NewsLoader(string link, CancellationToken cancellationToken)
        {
            var result = await _dateService.SeedNews(_mapper.Map<JsonConfigDTO>(_config), link, cancellationToken);

            return Ok(result);
        }
    }
}
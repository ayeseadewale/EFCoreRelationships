
using Microsoft.AspNetCore.Mvc;

namespace EFCoreRelationships.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;

        public CharacterController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _context.Character
                .Where(c => c.UserId == userId)
                .Include(c=>c.Weapon)
                .Include(c => c.Skills)
                .ToListAsync();
            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDTO request)
        {
            var user = await _context.User.FindAsync(request.UserId);
            if (user == null)
            
                return NotFound();
            
            var newcharacter = new Character
            {
                Name = request.Name,
                Rpg = request.Rpg,
                User = user
            };
            _context.Character.Add(newcharacter);
            await _context.SaveChangesAsync();
            return await Get(newcharacter.UserId);
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDTO request)
        {
            var character = await _context.Character.FindAsync(request.CharacterId);
            if (character == null)

                return NotFound();

            var newweapon = new Weapon
            {
                Name = request.Name,
                Damage = request.Damage,
                Character = character
            };
            _context.Weapon.Add(newweapon);
            await _context.SaveChangesAsync();
            return character;
        }
        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDTO request)
        {
            var character = await _context.Character
                .Where(c => c.Id == request.CharacterId)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync();
            
            if (character == null)

                return NotFound();
            
            var skill = await _context.Skill.FindAsync(request.SkillId);
            if (skill == null)

                return NotFound();

            character.Skills.Add(skill);
           
            await _context.SaveChangesAsync();
            return character;
        }
    }
}

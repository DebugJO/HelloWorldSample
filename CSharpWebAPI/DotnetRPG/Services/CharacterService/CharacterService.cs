using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotnetRPG.DTO.Character;
using DotnetRPG.Models;

namespace DotnetRPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character { ID = 1, Name = "Sam" }
        };
        private readonly IMapper mMapper;

        public CharacterService(IMapper mapper)
        {
            mMapper = mapper;

        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> AddCharacter(AddCharacterDTO newCharacter)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            Character character = mMapper.Map<Character>(newCharacter);
            character.ID = characters.Max(c => c.ID) + 1;
            characters.Add(character);
            serviceResponse.Data = (characters.Select(c => mMapper.Map<GetCharacterDTO>(c))).ToList();
            return await Task.Run(() => serviceResponse);
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> GetAllCharacters()
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            serviceResponse.Data = (characters.Select(c => mMapper.Map<GetCharacterDTO>(c))).ToList();
            return await Task.Run(() => serviceResponse);
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetCharacterByID(int id)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            serviceResponse.Data = mMapper.Map<GetCharacterDTO>(characters.FirstOrDefault(c => c.ID == id));
            return await Task.Run(() => serviceResponse);
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO)
        {
            ServiceResponse<GetCharacterDTO> serviceResponse = new ServiceResponse<GetCharacterDTO>();
            try
            {
                Character character = characters.FirstOrDefault(c => c.ID == updateCharacterDTO.ID);
                character.Name = updateCharacterDTO.Name;
                character.Class = updateCharacterDTO.Class;
                character.Defense = updateCharacterDTO.Defense;
                character.HitPoints = updateCharacterDTO.HitPoints;
                character.Intelligence = updateCharacterDTO.Intelligence;
                character.Strength = updateCharacterDTO.Strength;
                serviceResponse.Data = mMapper.Map<GetCharacterDTO>(character);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return await Task.Run(() => serviceResponse);
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDTO>> serviceResponse = new ServiceResponse<List<GetCharacterDTO>>();
            try
            {
                Character character = characters.First(c => c.ID == id);
                characters.Remove(character);
                serviceResponse.Data = (characters.Select(c => mMapper.Map<GetCharacterDTO>(c))).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return await Task.Run(() => serviceResponse);
        }
    }
}
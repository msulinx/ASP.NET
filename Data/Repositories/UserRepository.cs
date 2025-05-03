using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class UserRepository(AppDbContext context) 
    : BaseRepository<UserEntity, Member>(context), IUserRepository
{
    // Nu är det en riktig klass, och den kan använda alla metoder från BaseRepository
}
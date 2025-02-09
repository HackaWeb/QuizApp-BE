﻿using QuizApp.Domain.Models;
using QuizApp.Infrastructure.Repositories;

namespace QuizApp.Infrastructure;

public interface IUnitOfWork
{
    IRepository<Quiz> QuizRepository { get; }

    IRepository<QuizHistory> QuizHistoryRepository { get; }

    Task SaveEntitiesAsync(CancellationToken cancellationToken = default);
}

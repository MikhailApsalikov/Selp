namespace Example.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Entities;
    using Entities.Enums;

    public class TestDataInitializer : DropCreateDatabaseIfModelChanges<ExampleDbContext>
    {
        private readonly Random random = new Random();

        protected override void Seed(ExampleDbContext context)
        {
            InitializeRegions(context);
            InitializeTestParties(context);
            InitializeTestUsers(context);
            InitializeTestPolicies(context);
        }

        private void InitializeTestPolicies(ExampleDbContext context)
        {
            if (context.Policies.Any())
            {
                return;
            }

            IEnumerable<Policy> policies = Enumerable.Range(1, 500).Select(i => new Policy
            {
                UserId = "admin",
                CreatedDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(5).AddYears(1).AddSeconds(-1),
                InsurancePremium = random.Next(200, 500)*10,
                InsuranceSum = random.Next(5, 20)*100000,
                RegionId = random.Next(1, 80),
                StartDate = DateTime.Now.AddDays(5),
                Status = PolicyStatus.Actual,
                Serial = "EEE",
                Number = i.ToString("0000000000")
            });

            context.Policies.AddRange(policies);
            context.SaveChanges();
        }

        private void InitializeTestUsers(ExampleDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var users = new List<User>
            {
                new User
                {
                    Id = "admin",
                    Password = "demo"
                },
                new User
                {
                    Id = "user",
                    Password = "user"
                },
                new User
                {
                    Id = "inactive",
                    Password = "inactive",
                    IsInactive = true
                }
            };

            context.Users.AddRange(users);
        }

        private void InitializeTestParties(ExampleDbContext context)
        {
            if (context.Parties.Any())
            {
                return;
            }

            var parties = new List<Party>
            {
                new Party
                {
                    Address = "г.Саратов ул.Антонова д.25. кв.31",
                    BirthDate = new DateTime(1992, 12, 31),
                    Email = "dh@as.com",
                    FirstName = "Антон",
                    LastName = "Зеленцов",
                    MiddleName = "Петрович",
                    Phone = "+7915468741"
                },
                new Party
                {
                    Address = "г.Саратов ул.Антонова д.25. кв.32",
                    BirthDate = new DateTime(1970, 1, 1),
                    Email = "a@b.com",
                    FirstName = "Борис",
                    LastName = "Иванов",
                    MiddleName = "Романович",
                    Phone = "+7123546678"
                },
                new Party
                {
                    Address = "улица Пушкина, д.24 кв.5",
                    BirthDate = new DateTime(1990, 6, 8),
                    Email = "doctor@bk.ru",
                    FirstName = "Петр",
                    LastName = "Петров",
                    MiddleName = "Петрович",
                    Phone = "+79177894561"
                },
                new Party
                {
                    Address = "г.Саратов ул.Соколовая д.52. кв.11",
                    BirthDate = new DateTime(1987, 1, 2),
                    Email = "patient@bk.ru",
                    FirstName = "Елена",
                    LastName = "Иванова",
                    MiddleName = "Николаевна",
                    Phone = "+7915464321"
                },
                new Party
                {
                    Address = "ул. Батинская, дом 91, квартира 222",
                    BirthDate = new DateTime(1986, 6, 8),
                    Email = "oculist@bk.ru",
                    FirstName = "Зинаида",
                    LastName = "Пономарёва",
                    MiddleName = "Григорьевна",
                    Phone = "8 (908) 205-31-12"
                },
                new Party
                {
                    Address = "г. Камешково, ул. Голландская, дом 18, квартира 182",
                    BirthDate = new DateTime(1981, 10, 19),
                    Email = "isabella@bk.ru",
                    FirstName = "Изабелла",
                    LastName = "Наумова",
                    MiddleName = "Кирилловна",
                    Phone = "8 (908) 205-31-13"
                },
                new Party
                {
                    Address = "г. Седельниково, ул. Декабристов, дом 49, квартира 283",
                    BirthDate = new DateTime(1976, 12, 3),
                    Email = "gorodnov@bk.ru",
                    FirstName = "Агафон",
                    LastName = "Городнов",
                    MiddleName = "Владимирович",
                    Phone = "8 (928) 287-74-96"
                },
                new Party
                {
                    Address = "г. Пижанка, ул. Беговая 3-я, дом 63, квартира 64",
                    BirthDate = new DateTime(1969, 7, 13),
                    Email = "alexandrov@bk.ru",
                    FirstName = "Исай",
                    LastName = "Александров",
                    MiddleName = "Максимович",
                    Phone = "8 (978) 843-87-66"
                },
                new Party
                {
                    Address = "г. Усть-Омчуг, ул. Адмирала Лазарева, дом 71, квартира 129",
                    BirthDate = new DateTime(1991, 12, 25),
                    Email = "yog@bk.ru",
                    FirstName = "Вячеслав",
                    LastName = "Баландин",
                    MiddleName = "Сергеевич",
                    Phone = "8 (905) 936-16-48"
                },
                new Party
                {
                    Address = "г. Каракулино, ул. Веселова, дом 57, квартира 55",
                    BirthDate = new DateTime(1992, 8, 20),
                    Email = "oxana@bk.ru",
                    FirstName = "Оксана",
                    LastName = "Ичёткина",
                    MiddleName = "Матвеевна",
                    Phone = "8 (905) 459-26-71"
                },
                new Party
                {
                    Address = "г. Каракулино, ул. Веселова, дом 57, квартира 55",
                    BirthDate = new DateTime(1992, 9, 6),
                    Email = "luda@bk.ru",
                    FirstName = "Людмила",
                    LastName = "Соболева",
                    MiddleName = "Антоновна",
                    Phone = "8 (900) 685-31-46"
                },
                new Party
                {
                    Address = "г. Усть-Донецкий, ул. Базовская, дом 7, квартира 31",
                    BirthDate = new DateTime(1969, 7, 3),
                    Email = "katya@bk.ru",
                    FirstName = "Катерина",
                    LastName = "Трифонова",
                    MiddleName = "Владиславовна",
                    Phone = "8 (900) 387-44-52"
                },
                new Party
                {
                    Address = "г. Матросово, ул. Хрущёва, дом 88, квартира 282",
                    BirthDate = new DateTime(1992, 12, 31),
                    Email = "daniil@gmail.com",
                    FirstName = "Даниил",
                    LastName = "Васютин",
                    MiddleName = "Русланович",
                    Phone = "8 (979) 938-44-98"
                }
            };
            context.Parties.AddRange(parties);
            context.SaveChanges();
        }

        private void InitializeRegions(ExampleDbContext context)
        {
            if (context.Regions.Any())
            {
                return;
            }

            var regions = new List<Region>
            {
                new Region {Name = "Республика Адыгея"},
                new Region {Name = "Республика Башкортостан"},
                new Region {Name = "Республика Бурятия"},
                new Region {Name = "Республика Алтай"},
                new Region {Name = "Республика Дагестан"},
                new Region {Name = "Республика Ингушетия"},
                new Region {Name = "Кабардино - Балкарская Республика"},
                new Region {Name = "Республика Калмыкия"},
                new Region {Name = "Республика Карачаево-Черкесия"},
                new Region {Name = "Республика Карелия"},
                new Region {Name = "Республика Коми"},
                new Region {Name = "Республика Марий Эл"},
                new Region {Name = "Республика Мордовия"},
                new Region {Name = "Республика Саха(Якутия)"},
                new Region {Name = "Республика Северная Осетия - Алания"},
                new Region {Name = "Республика Татарстан"},
                new Region {Name = "Республика Тыва"},
                new Region {Name = "Удмуртская Республика"},
                new Region {Name = "Республика Хакасия"},
                new Region {Name = "Чеченская республика"},
                new Region {Name = "Чувашская Республика"},
                new Region {Name = "Алтайский край"},
                new Region {Name = "Краснодарский край"},
                new Region {Name = "Красноярский край"},
                new Region {Name = "Приморский край"},
                new Region {Name = "Ставропольский край"},
                new Region {Name = "Хабаровский край"},
                new Region {Name = "Амурская область"},
                new Region {Name = "Архангельская область"},
                new Region {Name = "Астраханская область"},
                new Region {Name = "Белгородская область"},
                new Region {Name = "Брянская область"},
                new Region {Name = "Владимирская область"},
                new Region {Name = "Волгоградская область"},
                new Region {Name = "Вологодская область"},
                new Region {Name = "Воронежская область"},
                new Region {Name = "Ивановская область"},
                new Region {Name = "Иркутская область"},
                new Region {Name = "Калининградская область"},
                new Region {Name = "Калужская область"},
                new Region {Name = "Камчатский край"},
                new Region {Name = "Кемеровская область"},
                new Region {Name = "Кировская область"},
                new Region {Name = "Костромская область"},
                new Region {Name = "Курганская область"},
                new Region {Name = "Курская область"},
                new Region {Name = "Ленинградская область"},
                new Region {Name = "Липецкая область"},
                new Region {Name = "Магаданская область"},
                new Region {Name = "Московская область"},
                new Region {Name = "Мурманская область"},
                new Region {Name = "Нижегородская область"},
                new Region {Name = "Новгородская область"},
                new Region {Name = "Новосибирская область"},
                new Region {Name = "Омская область"},
                new Region {Name = "Оренбургская область"},
                new Region {Name = "Орловская область"},
                new Region {Name = "Пензенская область"},
                new Region {Name = "Пермский край"},
                new Region {Name = "Псковская область"},
                new Region {Name = "Ростовская область"},
                new Region {Name = "Рязанская область"},
                new Region {Name = "Самарская область"},
                new Region {Name = "Саратовская область"},
                new Region {Name = "Сахалинская область"},
                new Region {Name = "Свердловская область"},
                new Region {Name = "Смоленская область"},
                new Region {Name = "Тамбовская область"},
                new Region {Name = "Тверская область"},
                new Region {Name = "Томская область"},
                new Region {Name = "Тульская область"},
                new Region {Name = "Тюменская область"},
                new Region {Name = "Ульяновская область"},
                new Region {Name = "Челябинская область"},
                new Region {Name = "Забайкальский край"},
                new Region {Name = "Ярославская область"},
                new Region {Name = "Еврейская автономная область"},
                new Region {Name = "Ненецкий автономный округ"},
                new Region {Name = "Ханты-Мансийский автономный округ - Югра"},
                new Region {Name = "Чукотский автономный округ"},
                new Region {Name = "Ямало-Ненецкий автономный округ"},
                new Region {Name = "Республика Крым"}
            };

            context.Regions.AddRange(regions);
            context.SaveChanges();
        }
    }
}
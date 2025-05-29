using PetShelter.Model.Core;

namespace PetShelter.Model.Data;

public static class DataInitializer
{
    private static readonly JsonSerializer _jsonSerializer = new JsonSerializer();
    private const string SHELTERS_FILE = "shelters";
    
    public static List<Shelter> CreateInitialData()
    {
        var shelters = new List<Shelter>
        {
            new("Добрые лапки", 15, true),
            new("Верный друг", 10, false),
            new("Надежда", 20, true),
            new("Любимец", 12, false)
        };

        var pets = new List<Pet>
        {
            // Собаки
            new Dog("Рекс", 3, 15.5, false, true, "Сухой корм"),
            new Dog("Барон", 5, 20.0, true, true, "Натуральный корм"),
            new Dog("Бобик", 2, 12.3, false, false, "Смешанный корм"),
            new Dog("Шарик", 4, 18.7, true, true, "Премиум корм"),
            new Dog("Альфа", 6, 25.0, false, true, "Диетический корм"),

            // Кошки
            new Cat("Мурка", 2, 4.5, false, true, "Ищет дом"),
            new Cat("Пушок", 3, 5.2, true, true, "На карантине"),
            new Cat("Васька", 1, 3.0, false, false, "Готов к адопции"),
            new Cat("Снежок", 4, 4.8, true, true, "В семье"),
            new Cat("Рыжик", 2, 4.0, false, true, "На лечении"),
            new Cat("Багира", 5, 4.3, true, false, "Ищет дом"),
            new Cat("Люся", 1, 3.5, false, true, "Готов к адопции"),


            // Кролики
            new Rabbit("Ушастик", 1, 2.0, false, "Вислоухий", false),
            new Rabbit("Пушистик", 2, 3.5, true, "Прямоухий", true),
            new Rabbit("Хлопок", 1, 1.8, false, "Вислоухий", false),
            new Rabbit("Морковка", 3, 4.2, true, "Прямоухий", true),
            new Rabbit("Попрыгун", 2, 2.5, false, "Вислоухий", false)
        };

        // Распределение питомцев по приютам
        try
        {
            // Распределяем животных по приютам
            // Приют "Добрые лапки" (с открытой территорией)
            shelters[0].AddPet(pets[0]); // Рекс
            shelters[0].AddPet(pets[5]); // Мурка
            shelters[0].AddPet(pets[12]); // Ушастик
            shelters[0].AddPet(pets[1]); // Барон
            shelters[0].AddPet(pets[6]); // Пушок

            // Приют "Верный друг" (без открытой территории)
            shelters[1].AddPet(pets[2]); // Грей
            shelters[1].AddPet(pets[7]); // Васька
            shelters[1].AddPet(pets[14]); // Морковка
            shelters[1].AddPet(pets[11]); // Люся

            // Приют "Надежда" (с открытой территорией)
            shelters[2].AddPet(pets[3]); // Шарик
            shelters[2].AddPet(pets[8]); // Снежок
            shelters[2].AddPet(pets[13]); // Снежинка
            shelters[2].AddPet(pets[15]); // Пушистик
            shelters[2].AddPet(pets[9]); // Рыжик
            shelters[2].AddPet(pets[4]); // Альфа

            // Приют "Любимец" (без открытой территории) — добавляем только тех, кто не боится замкнутого пространства
            var closedShelter = shelters[3];
            foreach (var pet in new[] { pets[10], pets[16] })
            {
                 if (!pet.Claustrophobic)
                 {
                     closedShelter.AddPet(pet);
                 }
                 // иначе пропускаем
            }

            // Сохраняем начальные данные в JSON
            _jsonSerializer.Serialize(SHELTERS_FILE, shelters);

            return shelters;
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при инициализации данных: {ex.Message}", ex);
        }
    }

    public static List<Shelter> LoadData()
    {
        try
        {
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<List<Shelter>>(SHELTERS_FILE);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Инициализация данных после ошибки: {ex.Message}");
            
            // при любом сбое — создаём и возвращаем начальные данные
            var initialData = CreateInitialData();
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(SHELTERS_FILE, initialData);
            return initialData;
        }
    }
}
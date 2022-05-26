namespace AldaTech_api.Models;

public class Component
{
	public int Id {get; set; }
	public string Type {get; set; } // Аналог name - бывает Message, Fork, Transition ...
	public List<Component> Props {get; set; }

}


// Максимальная глубина компонентов 
// - компоненты 
// 	- Развилка 
// 		- Цель (дейстиве)

// - компоненты 
// 	- текст с кнопками 
// 		- Сообщение 
// 		- Ряд кнопок 
// 			- Кнопка 
// 			- Кнопка 
// 		- Ряд кнопок 
// 			- Кнопка 
// 			- Кнопка 

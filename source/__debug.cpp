#include <generation_procedurale.hpp>

#include <windows.h>
#include <chrono>

void construction_console_log()
{
	CONSOLE_SCREEN_BUFFER_INFO infoConsole;
	FILE* fichierConsole;
	AllocConsole();
	GetConsoleScreenBufferInfo(GetStdHandle(STD_OUTPUT_HANDLE), &infoConsole);
	infoConsole.dwSize.Y = 500;
	SetConsoleScreenBufferSize(GetStdHandle(STD_OUTPUT_HANDLE), infoConsole.dwSize);

	freopen_s(&fichierConsole, "CONOUT$", "w", stdout); // todo rajouter cin ?
	return;
}
void log(std::string message)
{
	//CallbackLog(message.length(), (char*)message.data());
	std::cout << message << std::endl;
	return;
}
void destruction_console_log()
{
	FreeConsole();
	return;
}








static std::map<std::string, std::chrono::steady_clock::time_point> listePointsDemarrage;

void demarrer_profilage(std::string nom_instance)
{
	listePointsDemarrage[nom_instance] = std::chrono::steady_clock::now();
	return;
}
void arreter_profilage(std::string nom_instance)
{
	if (!listePointsDemarrage.contains(nom_instance))
	{
		log("erreur : nom d'instance non reconnue par le profilage");
	}
	std::chrono::steady_clock::time_point end = std::chrono::steady_clock::now();
	auto duree = std::chrono::duration_cast<std::chrono::microseconds>(end - listePointsDemarrage[nom_instance]);
	float f = (float)duree.count() / 1000.;
	log("profilage : [" + nom_instance + "] " + std::to_string(f) + " ms");
	listePointsDemarrage.erase(nom_instance);
	return;
}
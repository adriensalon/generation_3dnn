#include <generation_procedurale.hpp>

void preparation::interrompre_entrainement_texture()
{
	demarrer_profilage("interrompre_entrainement_texture");

	projet_actuel.devra_arreter_entrainement_au_prochain_tick.store(true);
	projet_actuel.thread_entrainement_texture.join();

	arreter_profilage("interrompre_entrainement_texture");
	return;
}
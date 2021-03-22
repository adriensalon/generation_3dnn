#include <generation_procedurale.hpp>

resultats_entrainement_texture preparation::obtenir_resultats_entrainement_texture()
{
	demarrer_profilage("obtenir_resultats_entrainement_texture");

	if (!projet_actuel.resultats.nouveau_tick)
	{
		arreter_profilage("obtenir_resultats_entrainement_texture");
		return projet_actuel.resultats;
	}

	while (!projet_actuel.ecriture_resultats_entrainement_en_cours.load())
	{
		log("message : concurrence entre les threads");
	}

	projet_actuel.resultats.nouveau_tick = false;

	arreter_profilage("obtenir_resultats_entrainement_texture");

	return projet_actuel.resultats;
}
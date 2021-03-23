#include <generation_procedurale.hpp>

#include "texture_procedurale.h"

void construction_matlab()
{
	log("init matlab");
	if (!mclInitializeApplication(NULL, 0)) {
		log("could not initialize the application");
		return;
	}
	if (!texture_proceduraleInitialize()) {
		log("erreur : [Matlab] could not initialize the library");
		return;
	}
}

void destruction_matlab()
{
	texture_proceduraleTerminate();
	mclTerminateApplication();
	return;
}
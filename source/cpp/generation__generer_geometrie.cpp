#include "GPROC.h"


geometrie geometrie_extension(
	const geometrie& g1, const std::vector<int>& indices1,
	const geometrie& g2, const std::vector<int>& indices2,
	const float espacementX, const float espacementY, const float espacementZ)
{
	auto nombreVerticesPartages = (int)indices1.size();
	auto nombreVerticesG1 = (int)g1.positions.size();
	auto nombreVerticesG2 = (int)g2.positions.size();

	auto g1C = geometrie();
	g1C = g1;

	auto g2T = geometrie();
	g2T = g2;// mesh deplace

	transformer_geometrie(
		g2T,
		reel3(espacementX, 0., 0.),
		reel3(0., 0., 0.),
		reel3(1., 1., 1.));


	auto xA = g1C.positions[indices1[0]].x;
	auto xB = g2T.positions[indices2[0]].x;

	auto gMilieu = geometrie(); // mesh milieu
	auto deuxNombreVerticesPartagees = 2 * nombreVerticesPartages;
	gMilieu.normales.resize(deuxNombreVerticesPartagees, reel3(0., 0., 0.));
	gMilieu.uvs.resize(deuxNombreVerticesPartagees, reel2(0., 0.));
	gMilieu.positions.resize(deuxNombreVerticesPartagees);
	for (auto iVertex = 0; iVertex < nombreVerticesPartages; iVertex++) // vertices
	{
		auto indiceG1 = indices1[iVertex];

		//Log(std::to_string(g1.positionsY[indices1[iVertex]]) + "    " + std::to_string(g2.positionsY[indices2[iVertex]]));

		//auto indiceG1Plus = indices1[iVertex + 1];
		gMilieu.positions[iVertex] = reel3(
			xA, 
			g1C.positions[indiceG1].y, 
			g1C.positions[indiceG1].z);
	}
	for (auto iVertex = 0; iVertex < nombreVerticesPartages; iVertex++)
	{
		auto indiceG2 = indices2[iVertex];
		auto indicePlusNombreVerticesPartagees = iVertex + nombreVerticesPartages;
		gMilieu.positions[indicePlusNombreVerticesPartagees] = reel3(
			xB, 
			g2T.positions[indiceG2].y, 
			g2T.positions[indiceG2].z);
	}

	for (auto iVertex = 0; iVertex < nombreVerticesPartages - 1; iVertex++) // triangles
	{
		auto indiceG2 = indices2[iVertex];


		auto a = iVertex;
		auto b = iVertex + 1;
		auto c = iVertex + 1 + nombreVerticesPartages;
		auto d = iVertex + nombreVerticesPartages;
		gMilieu.indices.push_back(entier3(a, b, c));
		gMilieu.indices.push_back(entier3(c, d, a));
	}
	auto a = nombreVerticesPartages - 1;
	auto b = 0;
	auto c = nombreVerticesPartages;
	auto d = 2 * nombreVerticesPartages - 1;
	gMilieu.indices.push_back(entier3(a, b, c));
	gMilieu.indices.push_back(entier3(c, d, a));

	auto v = std::vector<geometrie>();
	v.push_back(g1C);
	v.push_back(g2T);
	v.push_back(gMilieu);
	auto gComb = geometrie();
	gComb = combiner_geometries(v);


	sauvegarder_fichier_off("C:/Users/adri/Desktop/combinaison.off", gComb);

	return gComb;
}










geometrie generation::generer_geometrie(
	const sauvegarde_geometrie& s,
	const float taille_x)
{
	demarrer_profilage("generer_geometrie");
	// geometrie
	auto tailleXBplusC = s.ajustement_fin_x - s.ajustement_debut_x;
	auto espaceBoucle = taille_x - (s.boiteX - s.ajustement_fin_x) - (s.ajustement_debut_x);
	auto nombreLoop = (int)std::floor(espaceBoucle / tailleXBplusC);
	float espaceExt = espaceBoucle - (float)nombreLoop * tailleXBplusC;
	float tailleExt = espaceExt / nombreLoop;
	float tailleBoucle = espaceBoucle / nombreLoop;

	auto gX = s.a;
	auto gY = s.b;
	auto gZ = s.c;
	auto gW = s.d;

	auto loop = geometrie_extension(
		gY, 
		s.b_ligne_decoupe_x_b, 
		gZ, 
		s.c_ligne_decoupe_x_a, 
		tailleExt, 
		0., 
		0.); // loop originale

	auto vLoop = std::vector<geometrie>(nombreLoop);
	for (auto i = 0; i < nombreLoop; i++)
	{
		vLoop[i] = loop;
		transformer_geometrie(
			loop,
			reel3(tailleBoucle, 0., 0.),
			reel3(0., 0., 0.),
			reel3(1., 1., 1.));

	}
	auto toutesLesLoop = combiner_geometries(vLoop);

	sauvegarder_fichier_off("C:/Users/adri/Desktop/loops.off", toutesLesLoop);

	transformer_geometrie(
		gW,
		reel3(espaceBoucle - tailleXBplusC, 0., 0.),
		reel3(0., 0., 0.),
		reel3(1., 1., 1.));

	auto vFinal = std::vector<geometrie>();
	vFinal.push_back(gX);
	vFinal.push_back(toutesLesLoop);
	vFinal.push_back(gW);

	/*vFinal.push_back(s.a);
	vFinal.push_back(s.b);
	vFinal.push_back(s.c);
	vFinal.push_back(s.d);*/
	auto g_f = combiner_geometries(vFinal);

	arreter_profilage("generer_geometrie");
	return g_f;
}
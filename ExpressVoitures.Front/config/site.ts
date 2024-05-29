export type SiteConfig = typeof siteConfig;

export const siteConfig = {
  name: "Express Voitures - Inventaire",
  description: "Gestion d'inventaire de Express Voiture",
  navItems: [
    {
      label: "Dashboard",
      href: "/",
    }
  ],
  navMenuItems: [
    {
      label: "Profil",
      href: "/profile",
    },
    {
      label: "Se déconnecter",
      href: "/logout",
    },
  ],
};

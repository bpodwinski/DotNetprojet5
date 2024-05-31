export type SiteConfig = typeof siteConfig;

export const siteConfig = {
  name: "Express Voitures",
  description: "...",
  navItems: [
    {
      label: "Dashboard",
      href: "/dashboard",
    },
    {
      label: "About",
      href: "/about",
    },
  ],
  navMenuItems: [
    {
      label: "Profile",
      href: "/profile",
    },
    {
      label: "Logout",
      href: "/logout",
    },
  ],
  links: {
    github: "https://github.com/bpodwinski/DotNetprojet5",
  },
};

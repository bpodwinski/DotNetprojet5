"use client";

import React, { useState, useEffect } from 'react';
import {
  Navbar as NextUINavbar,
  NavbarContent,
  NavbarMenuToggle,
  NavbarBrand,
  NavbarItem,
} from "@nextui-org/navbar";
import { link as linkStyles } from "@nextui-org/theme";
import { useRouter } from 'next/navigation';
import NextLink from "next/link";
import clsx from "clsx";

import {Button, ButtonGroup} from "@nextui-org/button";
import {Link} from "@nextui-org/link";
import { siteConfig } from "@/config/site";
import { ThemeSwitch } from "@/components/theme-switch";
import {
  Logo,
} from "@/components/icons";
import {Image} from "@nextui-org/image";
import {  Dropdown,  DropdownTrigger,  DropdownMenu,  DropdownSection,  DropdownItem} from "@nextui-org/dropdown";
import {Avatar, AvatarGroup, AvatarIcon} from "@nextui-org/avatar";
import {User} from "@nextui-org/user";

export const Navbar = () => {
  const router = useRouter();
  const [hasToken, setHasToken] = useState(false);
  const [userInfo, setUserInfo] = useState({ firstname: '', lastname: '' });

  useEffect(() => {
    if (typeof window !== 'undefined') {
      const id = localStorage.getItem('id');
      const token = localStorage.getItem('token');
      setHasToken(!!token);

      const headers = new Headers({
        'Content-Type': 'application/json',
        'Authorization': token ? `Bearer ${token}` : ''
      });

      if (!token) {
        router.push('/login');
      }

      fetch('http://192.168.1.101:5000/user/' + id, { headers })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`Failed to fetch: ${response.statusText}`);
          }
          return response.json();
        })
        .then((data) => {
          setUserInfo({ firstname: data.firstname, lastname: data.lastname });
          console.log(data);
        })
        .catch((error) => {
          console.error("Error fetching vehicles:", error);
        });
    }

  }, []);

  const handleLogout = () => {
    localStorage.removeItem('id');
    localStorage.removeItem('token');
    setHasToken(false);
    router.push('/login');
  };

  return (
    <NextUINavbar maxWidth="xl" position="sticky">
      <NavbarContent className="basis-1/5 sm:basis-full" justify="start">
        <NavbarBrand as="li" className="gap-3 max-w-fit">
          <NextLink className="flex justify-start items-center gap-1" href="/">
            <Image
              width={50}
              alt="Express Voitures logo"
              src="logo.png"
            />
            <p className="font-bold text-inherit ml-2">Express Voitures</p>
          </NextLink>
        </NavbarBrand>
        <ul className="hidden lg:flex gap-4 justify-start ml-2">
          {siteConfig.navItems.map((item) => (
            <NavbarItem key={item.href}>
              <NextLink
                className={clsx(
                  linkStyles({ color: "foreground" }),
                  "data-[active=true]:text-primary data-[active=true]:font-medium",
                )}
                color="foreground"
                href={item.href}
              >
                {item.label}
              </NextLink>
            </NavbarItem>
          ))}
        </ul>
      </NavbarContent>

      <NavbarContent
        className="hidden sm:flex basis-1/5 sm:basis-full"
        justify="end"
      >
        <NavbarItem className="hidden sm:flex gap-2">
          <ThemeSwitch />
        </NavbarItem>

        {!hasToken ? (
          <NavbarItem>
            <Button as="a" color="primary" href="/login" variant="flat">
              Login
            </Button>
          </NavbarItem>
        ) : (
          <NavbarItem>
            <Dropdown placement="bottom-start">
              <DropdownTrigger>
                <User
                  as="button"
                  avatarProps={{
                    isBordered: true,
                    src: "https://i.pravatar.cc/100?u={`@${userInfo.email}`}",
                  }}
                  className="transition-transform"
                  description={`@${userInfo.firstname}${userInfo.lastname}`}
                  name={`${userInfo.firstname} ${userInfo.lastname}`}
                />
              </DropdownTrigger>
              <DropdownMenu aria-label="User Actions" variant="flat">
                <DropdownItem key="profile" className="h-14 gap-2">
                  <p className="font-bold">Signed in as</p>
                  <p className="font-bold">{`@${userInfo.firstname}${userInfo.lastname}`}</p>
                </DropdownItem>
                <DropdownItem key="logout" color="danger" onClick={handleLogout}>
                  Log Out
                </DropdownItem>
              </DropdownMenu>
            </Dropdown>
          </NavbarItem>
        )}
      </NavbarContent>

      <NavbarContent className="sm:hidden basis-1 pl-4" justify="end">
        <ThemeSwitch />
        <NavbarMenuToggle />
      </NavbarContent>

    </NextUINavbar>
  );
};

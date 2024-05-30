"use client";

import { NextResponse } from 'next/server';
import { useRouter } from 'next/navigation';
import React, { useState, useEffect } from 'react';
import {Card, CardHeader, CardBody, CardFooter} from "@nextui-org/card";
import {Input} from "@nextui-org/input";
import {Button, ButtonGroup} from "@nextui-org/button";

export default function LoginPage() {
  const router = useRouter()
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
    try {
      const response = await fetch('http://localhost:5000/user/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: email,
          password: password
        })
      });

      const responseData = await response.json();

      if (response.ok && responseData.token) {
        const id = responseData.id;
        const token = responseData.token;
        localStorage.setItem('id', id);
        localStorage.setItem('token', token);
        console.log('responseData :', responseData);
        router.push('/');
      }
      
      if (responseData.errors) {
        console.error('Erreur lors de la connexion :', responseData.errors);
      }

    } catch (error) {
      console.error('Erreur lors de l\'appel de l\'API :', error);
    }
  };

  return (
    <div className="w-full flex items-center justify-center">
        <Card className="w-[600px] p-6">
          <CardBody>
            <div className="flex flex-col items-center gap-4">
              <h1 className="pb-6 text-4xl" >Sign in</h1>
              
              <Input type="email" label="Email" value={email} onChange={(e) => setEmail(e.target.value)}  />
              <Input type="password" label="Password" value={password} onChange={(e) => setPassword(e.target.value)}  />

              <Button className="w-[120px]" onClick={handleLogin}>Sign in</Button>
            </div>
          </CardBody>
        </Card>
    </div>
  );
}

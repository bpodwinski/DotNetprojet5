import { SVGProps } from "react";

export type IconSvgProps = SVGProps<SVGSVGElement> & {
  size?: number;
};

export interface Vehicles {
  id: number;
  vin: string;
  year: number;
  brand: string;
  model: string;
  trim_level: string;
}

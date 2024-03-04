import { ComponentProps } from "react";

interface Props extends ComponentProps<"svg"> {}

export default function Logo(props: Props) {
  return (
    <svg
      width="704"
      height="511"
      viewBox="0 0 704 511"
      fill="none"
      preserveAspectRatio="xMidYMid meet"
      xmlns="http://www.w3.org/2000/svg"
      {...props}
    >
      <path
        d="M0 382H704V439C704 461.091 686.091 479 664 479H40C17.9086 479 0 461.091 0 439V382Z"
        fill="#2D3192"
      />
      <rect y="270" width="704" height="97" fill="#00AA5B" />
      <rect y="158" width="704" height="97" fill="#F5791F" />
      <path
        d="M0 90C0 67.9086 17.9086 50 40 50H662C684.091 50 702 67.9086 702 90V143H0V90Z"
        fill="#ED1B24"
      />
      <path
        d="M520.366 94.886L520.366 94.8862C523.821 98.61 526.528 102.364 528.442 107.043C530.35 111.706 531.678 117.794 531.697 126.392C529.204 188.904 509.854 294.299 427.442 362.282C351.008 421.834 266.337 426.558 226.417 428.069C218.041 427.361 212.406 425.835 208.234 423.895C204.167 422.004 200.997 419.493 197.8 416.076C190.629 406.959 188.67 403.001 187.864 399.96C187.445 398.377 187.237 396.651 187.109 393.919C187.071 393.118 187.035 392.012 186.995 390.731C186.929 388.687 186.85 386.196 186.724 383.785C193.154 316.023 200.144 244.606 274.431 162.993C307.873 129.642 356.321 109.405 399.579 97.6072C442.909 85.7899 479.405 82.8641 487.427 83.0809C496.206 83.318 502.433 84.2449 507.4 86.0324C512.161 87.7458 516.217 90.4137 520.366 94.886Z"
        stroke="white"
        strokeWidth="20"
      />
      <path
        d="M195.653 422C408 362 456.5 299 523.5 101.5"
        stroke="white"
        strokeWidth="10"
      />
      <path
        d="M283 429.5L237.941 387.888L192.672 339.597L204.499 296.905C242.158 338.315 247.466 357.733 326.88 414.985L283 429.5Z"
        fill="#FFFEFE"
      />
      <path
        d="M283 429.5L237.941 387.888L192.672 339.597L204.499 296.905C242.158 338.315 247.466 357.733 326.88 414.985L283 429.5Z"
        fill="#FFFEFE"
      />
      <path
        d="M441.101 87.1202L486.026 128.877L531.14 177.312L519.177 219.966C481.65 178.436 476.405 159.001 397.174 101.494L441.101 87.1202Z"
        fill="#FFFEFE"
      />
      <path
        d="M441.101 87.1202L486.026 128.877L531.14 177.312L519.177 219.966C481.65 178.436 476.405 159.001 397.174 101.494L441.101 87.1202Z"
        fill="#FFFEFE"
      />
      <path
        d="M241 269.5C279.598 207.173 309.008 176.984 373.93 136.898"
        stroke="white"
        strokeWidth="10"
        strokeLinecap="round"
      />
      <path
        d="M354 141L365 151"
        stroke="white"
        strokeWidth="20"
        strokeLinecap="round"
      />
      <path
        d="M329 157L340 168"
        stroke="white"
        strokeWidth="20"
        strokeLinecap="round"
      />
      <path
        d="M305 176L316 187"
        stroke="white"
        strokeWidth="20"
        strokeLinecap="round"
      />
      <path
        d="M279 199L291 209"
        stroke="white"
        strokeWidth="20"
        strokeLinecap="round"
      />
      <path
        d="M260 225L271 234"
        stroke="white"
        strokeWidth="20"
        strokeLinecap="round"
      />
      <path
        d="M243 249L255 261"
        stroke="white"
        strokeWidth="20"
        strokeLinecap="round"
      />
    </svg>
  );
}

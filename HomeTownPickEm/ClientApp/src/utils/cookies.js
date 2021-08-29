import Cookies from "js-cookie";

export const getUserCookie = () => {
  const userJson = Cookies.get("user");
  if (userJson) {
    return JSON.parse(userJson);
  }
  return null;
};

export const setUserCookie = (user) => {
  if (user) {
    Cookies.set("user", JSON.stringify(user), {expires: 30});
  } else {
    Cookies.remove("user");
  }
};
